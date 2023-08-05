using System.Text;

using Ardalis.Result;

using Clean.Architecture.Application.DTO.UserManagementModule.ApplicationUserAggregate;
using Clean.Architecture.Application.Interfaces.UserManagementModule;
using Clean.Architecture.Domain.UserManagementModule.ApplicationUserAggregate;
using Clean.Architecture.Domain.UserManagementModule.ApplicationUserAggregate.Events;
using Clean.Architecture.Domain.UserManagementModule.ApplicationUserAggregate.Specifications;
using Clean.Architecture.Domain.ValueObjects;
using Clean.Architecture.SharedKernel;
using Clean.Architecture.SharedKernel.Interfaces;
using Clean.Architecture.SharedKernel.Utils;

using MediatR;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

namespace Clean.Architecture.Application.Services.UserManagementModule;

public class ApplicationUserService : IApplicationUserService
{
  private readonly IUnitOfWork _unitOfWork;
  private readonly IReadRepository<ApplicationUser>? _readRepository;
  private readonly IRepository<ApplicationUser>? _repository;

  private readonly SignInManager<ApplicationUser> _signInManager;
  private readonly UserManager<ApplicationUser> _userManager;
  private readonly IUserStore<ApplicationUser> _userStore;
  private readonly IUserEmailStore<ApplicationUser> _emailStore;
  private readonly IUserPhoneNumberStore<ApplicationUser> _phoneNumberStore;

  private readonly IMediator _mediator;

  public ApplicationUserService(IUnitOfWork unitOfWork,
    SignInManager<ApplicationUser> signInManager,
    UserManager<ApplicationUser> userManager,
    IUserStore<ApplicationUser> userStore,
    IMediator mediator)
  {
    _unitOfWork = unitOfWork;
    _signInManager = signInManager;
    _userManager = userManager;
    _userStore = userStore;
    _emailStore = GetEmailStore();
    _phoneNumberStore = GetPhoneNumberStore();
    _mediator = mediator;

    _readRepository = _unitOfWork.GetIReadRepository<ApplicationUser>((byte)DbConnectionContext.Auth);
    _repository = _unitOfWork.GetIRepository<ApplicationUser>((byte)DbConnectionContext.Auth);
  }

  public async Task<Result<ApplicationUserDTO>> AddApplicationUserAsync(ApplicationUserDTO applicationUserDTO, ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      var aggregateToAdd = ApplicationUserFactory.CreateApplicationUser(applicationUserDTO.UserName, applicationUserDTO.Email, applicationUserDTO.EmailConfirmed, applicationUserDTO.PhoneNumber, applicationUserDTO.PhoneNumberConfirmed, (byte)RecordStatus.Approved, serviceHeader);

      var result = await _repository!.AddAsync(aggregateToAdd, cts.Token);
      await _unitOfWork.SaveAsync();
      var domainEvent = new ApplicationUserAddedEvent(result, string.Empty);
      await _mediator.Publish(domainEvent, cts.Token).ConfigureAwait(false);
      return Result.Success(_unitOfWork.MapTo<ApplicationUserDTO>(result));
    }
    catch (Exception ex)
    {
      // TODO: Log details here
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result<(IdentityResult, string UserId, string Code)>> CreateApplicationUserAsync(ApplicationUserDTO applicationUserDTO, ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      var user = CreateUser();
      user.FirstName = applicationUserDTO.FirstName;
      user.MiddleName = applicationUserDTO.MiddleName;
      user.LastName = applicationUserDTO.LastName;
      user.ProfilePicture = applicationUserDTO.ProfilePicture;

      var utcNow = DateTime.UtcNow;

      user.RecordStatus = (byte)RecordStatus.Approved;
      user.LastPasswordChangedDate = utcNow;
      user.CreatedDate = utcNow;
      user.CreatedBy = applicationUserDTO.CreatedBy ?? serviceHeader.ApplicationUserName!;

#if DEBUG
      applicationUserDTO.Password = DefaultSettings.Instance.RootPassword;
#else
applicationUserDTO.Password = PasswordGenerator.GeneratePassword(true, true, true, true, false, 8);
#endif

      await _userStore.SetUserNameAsync(user, applicationUserDTO.UserName, cancellationToken).ConfigureAwait(false);
      await _emailStore.SetEmailAsync(user, applicationUserDTO.Email, cancellationToken).ConfigureAwait(false);
      await _phoneNumberStore.SetPhoneNumberAsync(user, applicationUserDTO.PhoneNumber, cancellationToken).ConfigureAwait(false);

      var result = await _userManager.CreateAsync(user, applicationUserDTO.Password!);

      if (result.Succeeded)
      {
        var userId = await _userManager.GetUserIdAsync(user);
        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        var domainEvent = new ApplicationUserAddedEvent(user, applicationUserDTO.Password);
        await _mediator.Publish(domainEvent, cts.Token).ConfigureAwait(false);

        return Result.Success<(IdentityResult, string, string)>((result, userId, code));
      }
      else
      {
        if (result.Errors.Any())
        {
          Result.Error(new[] { $"{string.Join(',', result.Errors.Select(e => $"{e.Code}: {e.Description}").ToList())}" });
        }
      }

      return Result.Success<(IdentityResult, string, string)>((result, string.Empty, string.Empty));
    }
    catch (Exception ex)
    {
      // TODO: Log details here
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result<IdentityResult>> ConfirmApplicationUserEmailAsync(string userId, string code, ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      var user = await _userManager.FindByIdAsync(userId);

      if (user != null)
      {
        code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));

        var result = await _userManager.ConfirmEmailAsync(user, code).ConfigureAwait(false);

        if (result.Succeeded)
        {
          var utcNow = DateTime.UtcNow;

          user.ModifiedDate = DateTime.UtcNow;
          user.ModifiedBy ??= serviceHeader.ApplicationUserName!;

          result = await _userManager.UpdateAsync(user).ConfigureAwait(false);

          var domainEvent = new ApplicationUserEmailConfirmedEvent(user);
          await _mediator.Publish(domainEvent, cts.Token).ConfigureAwait(false);
        }

        return Result.Success(result);
      }

      return Result.NotFound();
    }
    catch (Exception ex)
    {
      // TODO: Log details here
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result<SignInResult>> LoginApplicationUserAsync(AccountLoginBindingModel accountLoginBindingModel, ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      var user = !string.IsNullOrWhiteSpace(accountLoginBindingModel.UserName) ? await _userManager.FindByNameAsync(accountLoginBindingModel.UserName!) : await _userManager.FindByEmailAsync(accountLoginBindingModel.Email!);

      if (user != null)
      {
        var result = await _signInManager.PasswordSignInAsync(user!, accountLoginBindingModel.Password!, true, true);

        if (result.Succeeded)
        {
          user.LastLoggedInDate = DateTime.UtcNow;
          user.TransactionEnvironment = new TransactionEnvironment
          {
            EnvironmentIPAddress = user.TransactionEnvironment.EnvironmentIPAddress ?? serviceHeader.EnvironmentIPAddress,
            EnvironmentMACAddress = user.TransactionEnvironment.EnvironmentMACAddress ?? serviceHeader.EnvironmentMACAddress,
            EnvironmentMotherboardSerialNumber = user.TransactionEnvironment.EnvironmentMotherboardSerialNumber ?? serviceHeader.EnvironmentMotherboardSerialNumber,
            EnvironmentProcessorId = user.TransactionEnvironment.EnvironmentProcessorId ?? serviceHeader.EnvironmentProcessorId,
            EnvironmentUserName = user.TransactionEnvironment.EnvironmentUserName ?? serviceHeader.EnvironmentUserName,
            EnvironmentMachineName = user.TransactionEnvironment.EnvironmentMachineName ?? serviceHeader.EnvironmentMachineName,
            EnvironmentDomainName = user.TransactionEnvironment.EnvironmentDomainName ?? serviceHeader.EnvironmentDomainName,
            EnvironmentOSVersion = user.TransactionEnvironment.EnvironmentOSVersion ?? serviceHeader.EnvironmentOSVersion,

            ThirdPartyClientIPAddress = user.TransactionEnvironment.ClientIPAddress ?? serviceHeader.ClientIPAddress,

            ClientIPAddress = user.TransactionEnvironment.ClientIPAddress ?? serviceHeader.ClientIPAddress,
            ClientUserAgent = user.TransactionEnvironment.ClientUserAgent ?? serviceHeader.ClientUserAgent,
            ClientBrowser = user.TransactionEnvironment.ClientBrowser ?? serviceHeader.ClientBrowser?.ToString(),
            ClientLocation = user.TransactionEnvironment.ClientLocation ?? serviceHeader.ClientLocation?.ToString(),
          };

          await _userManager.UpdateAsync(user);

          var domainEvent = new ApplicationUserLoggedInEvent(user!);
          await _mediator.Publish(domainEvent, cts.Token).ConfigureAwait(false);

          return Result.Success(result);
        }

        return Result.Success(result);
      }

      return Result.NotFound();
    }
    catch (Exception ex)
    {
      // TODO: Log details here
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result<bool>> BulkInsertApplicationUsersAsync(List<ApplicationUserDTO> applicationUserDTOs, ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    var result = default(bool);

    try
    {
      if (_readRepository!.IsDatabaseSqlServer())
      {
        var applicationUsers = new List<ApplicationUser>();

        applicationUserDTOs.ForEach(applicationUserDTO =>
        {
          applicationUsers.Add(ApplicationUserFactory.CreateApplicationUser(applicationUserDTO.UserName, applicationUserDTO.Email, applicationUserDTO.EmailConfirmed, applicationUserDTO.PhoneNumber, applicationUserDTO.PhoneNumberConfirmed, (byte)RecordStatus.Approved, serviceHeader));
        });

        result = await _repository!.DatabaseBulkInsertAsync(applicationUsers, Utility.DbTableName<ApplicationUser>(), serviceHeader, cts.Token);

        result = await _unitOfWork.SaveAsync();
      }
      else
      {
        applicationUserDTOs.ForEach(async applicationUserDTO =>
        {
          result = await AddApplicationUserAsync(applicationUserDTO, serviceHeader, cts.Token) != null;
        });
      }

      return Result.Success(result);
    }
    catch (Exception ex)
    {
      // TODO: Log details here
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result> UpdateApplicationUserAsync(Guid applicationUserId, ApplicationUserDTO applicationUserDTO, ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      var aggregateToUpdate = await _repository!.GetByIdAsync(applicationUserId, cts.Token);
      if (aggregateToUpdate == null) return Result.NotFound();

      aggregateToUpdate = ApplicationUserFactory.CreateApplicationUser(applicationUserDTO.UserName, applicationUserDTO.Email, applicationUserDTO.EmailConfirmed, applicationUserDTO.PhoneNumber, applicationUserDTO.PhoneNumberConfirmed, (byte)RecordStatus.Approved, serviceHeader, aggregateToUpdate);

      await _repository!.UpdateAsync(aggregateToUpdate, cts.Token);
      await _unitOfWork.SaveAsync();
      var domainEvent = new ApplicationUserUpdatedEvent(aggregateToUpdate);
      await _mediator.Publish(domainEvent, cts.Token).ConfigureAwait(false);
      return Result.Success();
    }
    catch (Exception ex)
    {
      // TODO: Log details here
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result> DeleteApplicationUserAsync(Guid applicationUserId, ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      var aggregateToDelete = await _readRepository!.GetByIdAsync(applicationUserId, cts.Token);
      if (aggregateToDelete == null) return Result.NotFound();

      await _repository!.DeleteAsync(aggregateToDelete, cts.Token);
      var domainEvent = new ApplicationUserDeletedEvent(applicationUserId);
      await _mediator.Publish(domainEvent, cts.Token).ConfigureAwait(false);
      return Result.Success();
    }
    catch (Exception ex)
    {
      // TODO: Log details here
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result<ApplicationUserDTO>> FindApplicationUserAsync(Guid applicationUserId, ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      var aggregate = await _readRepository!.GetByIdAsync(applicationUserId, cts.Token);

      if (aggregate == null)
      {
        return Result.NotFound();
      }

      return new Result<ApplicationUserDTO>(_unitOfWork.MapTo<ApplicationUserDTO>(aggregate));
    }
    catch (Exception ex)
    {
      // TODO: Log details here
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result<List<ApplicationUserDTO>>> FindApplicationUsersAsync(ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      var applicationUserDTOs = await _readRepository!.ListAsync<ApplicationUserDTO>(cts.Token);

      if (applicationUserDTOs == null)
      {
        return Result.NotFound();
      }

      return new Result<List<ApplicationUserDTO>>(applicationUserDTOs);
    }
    catch (Exception ex)
    {
      // TODO: Log details here
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result<PageCollectionInfo<ApplicationUserDTO>>> GetApplicationUsersWithFiltersAndInPageAsync(string? searchString, int pageNumber, int pageSize, string sortColumn, string sortDirection, ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      var allApplicationUserSpec = new ApplicationUsersWithFiltersAndInPageSpec(searchString, sortColumn, sortDirection);

      var applicationUsers = await PaginatedIAggregateRootList<ApplicationUser, ApplicationUserDTO>.CreatePageCollectionInfoAsync(_unitOfWork, allApplicationUserSpec, pageNumber, pageSize, _unitOfWork.MapTo<ApplicationUserDTO>, cts.Token);

      if (applicationUsers != null)
      {
        if (applicationUsers.PageCollection == null)
        {
          return Result<PageCollectionInfo<ApplicationUserDTO>>.NotFound();
        }

        return new Result<PageCollectionInfo<ApplicationUserDTO>>(applicationUsers);
      }

      return Result<PageCollectionInfo<ApplicationUserDTO>>.NotFound("");
    }
    catch (Exception ex)
    {
      // TODO: Log details here
      return Result<PageCollectionInfo<ApplicationUserDTO>>.Error(new[] { ex.Message });
    }
  }

  #region Helper Methods

  private ApplicationUser CreateUser()
  {
    try
    {
      return Activator.CreateInstance<ApplicationUser>();
    }
    catch
    {
      throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
          $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
          $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
    }
  }

  private IUserEmailStore<ApplicationUser> GetEmailStore()
  {
    if (!_userManager.SupportsUserEmail)
    {
      throw new NotSupportedException("The default UI requires a user store with email support.");
    }
    return (IUserEmailStore<ApplicationUser>)_userStore;
  }

  private IUserPhoneNumberStore<ApplicationUser> GetPhoneNumberStore()
  {
    if (!_userManager.SupportsUserPhoneNumber)
    {
      throw new NotSupportedException("The default UI requires a user store with phone support.");
    }
    return (IUserPhoneNumberStore<ApplicationUser>)_userStore;
  }

  private IUserPasswordStore<ApplicationUser> GetPasswordStore()
  {
    if (!_userManager.SupportsUserPassword)
    {
      throw new NotSupportedException("The default UI requires a user store with password support.");
    }
    return (IUserPasswordStore<ApplicationUser>)_userStore;
  }

  #endregion
}

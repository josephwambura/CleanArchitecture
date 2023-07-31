using Ardalis.Result;
using Clean.Architecture.Application.DTO.UserManagementModule.ApplicationUserAggregate;
using Clean.Architecture.SharedKernel;
using Clean.Architecture.SharedKernel.Utils;

using Microsoft.AspNetCore.Identity;

namespace Clean.Architecture.Application.Interfaces.UserManagementModule;

public interface IApplicationUserService
{
  Task<Result<ApplicationUserDTO>> AddApplicationUserAsync(ApplicationUserDTO applicationUserDTO, ServiceHeader serviceHeader, CancellationToken cancellationToken = default);

  Task<Result<(IdentityResult, string UserId, string Code)>> CreateApplicationUserAsync(ApplicationUserDTO applicationUserDTO, ServiceHeader serviceHeader, CancellationToken cancellationToken = default);

  Task<Result<IdentityResult>> ConfirmApplicationUserEmailAsync(string userId, string code, ServiceHeader serviceHeader, CancellationToken cancellationToken = default);

  Task<Result<SignInResult>> LoginApplicationUserAsync(AccountLoginBindingModel accountLoginBindingModel, ServiceHeader serviceHeader, CancellationToken cancellationToken = default);

  Task<Result<bool>> BulkInsertApplicationUsersAsync(List<ApplicationUserDTO> applicationUserDTOs, ServiceHeader serviceHeader, CancellationToken cancellationToken = default);

  Task<Result> UpdateApplicationUserAsync(Guid applicationUserId, ApplicationUserDTO applicationUserDTO, ServiceHeader serviceHeader, CancellationToken cancellationToken = default);

  Task<Result> DeleteApplicationUserAsync(Guid applicationUserId, ServiceHeader serviceHeader, CancellationToken cancellationToken = default);

  Task<Result<ApplicationUserDTO>> FindApplicationUserAsync(Guid applicationUserId, ServiceHeader serviceHeader, CancellationToken cancellationToken = default);
  
  Task<Result<List<ApplicationUserDTO>>> FindApplicationUsersAsync(ServiceHeader serviceHeader, CancellationToken cancellationToken = default);

  Task<Result<PageCollectionInfo<ApplicationUserDTO>>> GetApplicationUsersWithFiltersAndInPageAsync(string? searchString, int pageNumber, int pageSize, string sortColumn, string sortDirection, ServiceHeader serviceHeader, CancellationToken cancellationToken = default);
}

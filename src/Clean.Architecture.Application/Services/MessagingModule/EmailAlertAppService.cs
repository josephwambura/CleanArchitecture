using Ardalis.Result;

using Clean.Architecture.Application.Core.Interfaces.MessagingModule;
using Clean.Architecture.Application.DTO.MessagingModule.EmailAlertAggregate;
using Clean.Architecture.Core.MessagingModule.EmailAlertAggregate;
using Clean.Architecture.Core.MessagingModule.EmailAlertAggregate.Events;
using Clean.Architecture.Core.MessagingModule.EmailAlertAggregate.Specifications;
using Clean.Architecture.Core.ValueObjects;
using Clean.Architecture.SharedKernel;
using Clean.Architecture.SharedKernel.Interfaces;
using Clean.Architecture.SharedKernel.Utils;

using MediatR;

namespace Clean.Architecture.Application.Core.Services.MessagingModule;

public class EmailAlertAppService : IEmailAlertAppService
{
  private readonly IUnitOfWork _unitOfWork;
  private readonly IReadRepository<EmailAlert>? _readRepository;
  private readonly IRepository<EmailAlert>? _repository;
  private readonly IMediator _mediator;

  public EmailAlertAppService(IUnitOfWork unitOfWork, IMediator mediator)
  {
    _unitOfWork = unitOfWork;
    _mediator = mediator;

    _readRepository = _unitOfWork.GetIReadRepository<EmailAlert>((byte)DbConnectionContext.LIVE);
    _repository = _unitOfWork.GetIRepository<EmailAlert>((byte)DbConnectionContext.LIVE);
  }

  public async Task<Result<EmailAlertDTO>> AddEmailAlertAsync(EmailAlertDTO emailAlertDTO, ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      var emailMessage = new EmailMessage(emailAlertDTO.EmailMessageTo!, emailAlertDTO.EmailMessageCC, emailAlertDTO.EmailMessageSubject!, emailAlertDTO.EmailMessageBody!, emailAlertDTO.EmailMessageAttachments, emailAlertDTO.EmailMessageIsBodyHtml, emailAlertDTO.EmailMessageSecurityCritical);

      var aggregateToAdd = EmailAlertFactory.CreateEmailAlert(emailAlertDTO.From!, emailMessage, emailAlertDTO.DLRStatus, emailAlertDTO.Reference, emailAlertDTO.Origin, emailAlertDTO.Catalyst, emailAlertDTO.Priority, emailAlertDTO.SendRetry, emailAlertDTO.RecordStatus, serviceHeader);

      var result = await _repository!.AddAsync(aggregateToAdd, cts.Token);
      await _unitOfWork.SaveAsync();
      var domainEvent = new EmailAlertAddedEvent(result);
      await _mediator.Publish(domainEvent, cts.Token).ConfigureAwait(false);
      return Result.Success(_unitOfWork.MapTo<EmailAlertDTO>(result));
    }
    catch (Exception ex)
    {
      // TODO: Log details here
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result<bool>> BulkInsertEmailAlertsAsync(List<EmailAlertDTO> emailAlertDTOs, ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    var result = default(bool);

    try
    {
      if (_readRepository!.IsDatabaseSqlServer())
      {
        var emailAlerts = new List<EmailAlert>();

        emailAlertDTOs.ForEach(emailAlertDTO =>
        {
          var emailMessage = new EmailMessage(emailAlertDTO.EmailMessageTo!, emailAlertDTO.EmailMessageCC, emailAlertDTO.EmailMessageSubject!, emailAlertDTO.EmailMessageBody!, emailAlertDTO.EmailMessageAttachments, emailAlertDTO.EmailMessageIsBodyHtml, emailAlertDTO.EmailMessageSecurityCritical);

          emailAlerts.Add(EmailAlertFactory.CreateEmailAlert(emailAlertDTO.From!, emailMessage, emailAlertDTO.DLRStatus, emailAlertDTO.Reference, emailAlertDTO.Origin, emailAlertDTO.Catalyst, emailAlertDTO.Priority, emailAlertDTO.SendRetry, emailAlertDTO.RecordStatus, serviceHeader));
        });

        result = await _repository!.DatabaseBulkInsertAsync(emailAlerts, Utility.DbTableName<EmailAlert>(), serviceHeader, cts.Token);

        result = await _unitOfWork.SaveAsync();
      }
      else
      {
        emailAlertDTOs.ForEach(async emailAlertDTO =>
        {
          result = await AddEmailAlertAsync(emailAlertDTO, serviceHeader, cts.Token) != null;
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

  public async Task<Result> UpdateEmailAlertAsync(Guid emailAlertId, EmailAlertDTO emailAlertDTO, ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      var aggregateToUpdate = await _repository!.GetByIdAsync(emailAlertId, cts.Token);
      if (aggregateToUpdate == null) return Result.NotFound();

      var emailMessage = new EmailMessage(emailAlertDTO.EmailMessageTo!, emailAlertDTO.EmailMessageCC, emailAlertDTO.EmailMessageSubject!, emailAlertDTO.EmailMessageBody!, emailAlertDTO.EmailMessageAttachments, emailAlertDTO.EmailMessageIsBodyHtml, emailAlertDTO.EmailMessageSecurityCritical);

      aggregateToUpdate = EmailAlertFactory.CreateEmailAlert(emailAlertDTO.From!, emailMessage, emailAlertDTO.DLRStatus, emailAlertDTO.Reference, emailAlertDTO.Origin, emailAlertDTO.Catalyst, emailAlertDTO.Priority, emailAlertDTO.SendRetry, emailAlertDTO.RecordStatus, serviceHeader, aggregateToUpdate);

      await _repository!.UpdateAsync(aggregateToUpdate, cts.Token);
      await _unitOfWork.SaveAsync();
      var domainEvent = new EmailAlertUpdatedEvent(aggregateToUpdate);
      await _mediator.Publish(domainEvent, cts.Token).ConfigureAwait(false);
      return Result.Success();
    }
    catch (Exception ex)
    {
      // TODO: Log details here
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result> MarkQueuedEmailAlertAsync(Guid emailAlertId, ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      var aggregateToUpdate = await _repository!.GetByIdAsync(emailAlertId, cts.Token);
      if (aggregateToUpdate == null) return Result.NotFound();

      aggregateToUpdate.DLRStatus = (byte)DLRStatus.Queued.Value;

      var emailMessage = new EmailMessage(aggregateToUpdate.EmailMessage.To!, aggregateToUpdate.EmailMessage.CC, aggregateToUpdate.EmailMessage.Subject!, aggregateToUpdate.EmailMessage.Body!, aggregateToUpdate.EmailMessage.Attachments, aggregateToUpdate.EmailMessage.IsBodyHtml, aggregateToUpdate.EmailMessage.SecurityCritical);

      aggregateToUpdate = EmailAlertFactory.CreateEmailAlert(aggregateToUpdate.From!, emailMessage, aggregateToUpdate.DLRStatus, aggregateToUpdate.Reference, aggregateToUpdate.Origin, aggregateToUpdate.Catalyst, aggregateToUpdate.Priority, aggregateToUpdate.SendRetry, aggregateToUpdate.RecordStatus, serviceHeader, aggregateToUpdate);

      await _repository!.UpdateAsync(aggregateToUpdate, cts.Token);
      await _unitOfWork.SaveAsync();
      var domainEvent = new EmailAlertUpdatedEvent(aggregateToUpdate);
      await _mediator.Publish(domainEvent, cts.Token).ConfigureAwait(false);
      return Result.Success();
    }
    catch (Exception ex)
    {
      // TODO: Log details here
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result> MarkDeliveredEmailAlertAsync(Guid emailAlertId, ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      var aggregateToUpdate = await _repository!.GetByIdAsync(emailAlertId, cts.Token);
      if (aggregateToUpdate == null) return Result.NotFound();

      aggregateToUpdate.DLRStatus = (byte)DLRStatus.Queued.Value;

      var emailMessage = new EmailMessage(aggregateToUpdate.EmailMessage.To!, aggregateToUpdate.EmailMessage.CC, aggregateToUpdate.EmailMessage.Subject!, aggregateToUpdate.EmailMessage.Body!, aggregateToUpdate.EmailMessage.Attachments, aggregateToUpdate.EmailMessage.IsBodyHtml, aggregateToUpdate.EmailMessage.SecurityCritical);

      aggregateToUpdate = EmailAlertFactory.CreateEmailAlert(aggregateToUpdate.From!, emailMessage, aggregateToUpdate.DLRStatus, aggregateToUpdate.Reference, aggregateToUpdate.Origin, aggregateToUpdate.Catalyst, aggregateToUpdate.Priority, aggregateToUpdate.SendRetry, aggregateToUpdate.RecordStatus, serviceHeader, aggregateToUpdate);

      await _repository!.UpdateAsync(aggregateToUpdate, cts.Token);
      await _unitOfWork.SaveAsync();
      var domainEvent = new EmailAlertUpdatedEvent(aggregateToUpdate);
      await _mediator.Publish(domainEvent, cts.Token).ConfigureAwait(false);
      return Result.Success();
    }
    catch (Exception ex)
    {
      // TODO: Log details here
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result> DeleteEmailAlertAsync(Guid emailAlertId, ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      var aggregateToDelete = await _readRepository!.GetByIdAsync(emailAlertId, cts.Token);
      if (aggregateToDelete == null) return Result.NotFound();

      await _repository!.DeleteAsync(aggregateToDelete, cts.Token);
      var domainEvent = new EmailAlertDeletedEvent(emailAlertId);
      await _mediator.Publish(domainEvent, cts.Token).ConfigureAwait(false);
      return Result.Success();
    }
    catch (Exception ex)
    {
      // TODO: Log details here
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result<EmailAlertDTO>> FindEmailAlertAsync(Guid emailAlertId, ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      var aggregate = await _readRepository!.GetByIdAsync(emailAlertId, cts.Token);

      if (aggregate == null)
      {
        return Result.NotFound();
      }

      return new Result<EmailAlertDTO>(_unitOfWork.MapTo<EmailAlertDTO>(aggregate));
    }
    catch (Exception ex)
    {
      // TODO: Log details here
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result<List<EmailAlertDTO>>> FindEmailAlertsAsync(ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      var emailAlertDTOs = await _readRepository!.ListAsync<EmailAlertDTO>(cts.Token);

      if (emailAlertDTOs == null)
      {
        return Result.NotFound();
      }

      return new Result<List<EmailAlertDTO>>(emailAlertDTOs);
    }
    catch (Exception ex)
    {
      // TODO: Log details here
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result<PageCollectionInfo<EmailAlertDTO>>> GetEmailAlertsWithFiltersAndInPageAsync(string? searchString, int pageNumber, int pageSize, string sortColumn, string sortDirection, ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      var allEmailAlertSpec = new EmailAlertsWithFiltersAndInPageSpec(searchString, sortColumn, sortDirection);

      var emailAlerts = await PaginatedIAggregateRootList<EmailAlert, EmailAlertDTO>.CreatePageCollectionInfoAsync(_unitOfWork, allEmailAlertSpec, pageNumber, pageSize, _unitOfWork.MapTo<EmailAlertDTO>, cts.Token);

      if (emailAlerts != null)
      {
        if (emailAlerts.PageCollection == null)
        {
          return Result<PageCollectionInfo<EmailAlertDTO>>.NotFound();
        }

        return new Result<PageCollectionInfo<EmailAlertDTO>>(emailAlerts);
      }

      return Result<PageCollectionInfo<EmailAlertDTO>>.NotFound("");
    }
    catch (Exception ex)
    {
      // TODO: Log details here
      return Result<PageCollectionInfo<EmailAlertDTO>>.Error(new[] { ex.Message });
    }
  }

  public async Task<Result<List<EmailAlertDTO>>> GetEmailAlertsByDLRStatusAndWithFiltersAndInPageAsync(byte[] dlrStatuses, string? searchString, int pageSize, string sortColumn, string sortDirection, ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      var allEmailAlertSpec = new EmailAlertsByDLRStatusAndWithFiltersAndInPageSpec(dlrStatuses, searchString, pageSize, sortColumn, sortDirection);

      var emailAlertDTOs = await _readRepository!.ListAsync<EmailAlertDTO>(allEmailAlertSpec, cts.Token);

      if (emailAlertDTOs == null)
      {
        return Result.NotFound();
      }

      return new Result<List<EmailAlertDTO>>(emailAlertDTOs);
    }
    catch (Exception ex)
    {
      // TODO: Log details here
      return Result.Error(new[] { ex.Message });
    }
  }
}

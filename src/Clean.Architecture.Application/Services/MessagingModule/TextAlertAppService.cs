using Ardalis.Result;

using Clean.Architecture.Application.Core.Interfaces.MessagingModule;
using Clean.Architecture.Application.DTO.MessagingModule.TextAlertAggregate;
using Clean.Architecture.Domain.MessagingModule.TextAlertAggregate;
using Clean.Architecture.Domain.MessagingModule.TextAlertAggregate.Events;
using Clean.Architecture.Domain.MessagingModule.TextAlertAggregate.Specifications;
using Clean.Architecture.Domain.ValueObjects;
using Clean.Architecture.SharedKernel;
using Clean.Architecture.SharedKernel.Interfaces;
using Clean.Architecture.SharedKernel.Utils;

using MediatR;

namespace Clean.Architecture.Application.Core.Services.MessagingModule;

public class TextAlertAppService : ITextAlertAppService
{
  private readonly IUnitOfWork _unitOfWork;
  private readonly IReadRepository<TextAlert>? _readRepository;
  private readonly IRepository<TextAlert>? _repository;
  private readonly IMediator _mediator;

  public TextAlertAppService(IUnitOfWork unitOfWork, IMediator mediator)
  {
    _unitOfWork = unitOfWork;
    _mediator = mediator;

    _readRepository = _unitOfWork.GetIReadRepository<TextAlert>((byte)DbConnectionContext.LIVE);
    _repository = _unitOfWork.GetIRepository<TextAlert>((byte)DbConnectionContext.LIVE);
  }

  public async Task<Result<TextAlertDTO>> AddTextAlertAsync(TextAlertDTO textAlertDTO, ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      var textMessage = new TextMessage(textAlertDTO.TextMessageRecipient!, textAlertDTO.TextMessageBody!, textAlertDTO.TextMessageSecurityCritical);

      var aggregateToAdd = TextAlertFactory.CreateTextAlert(textMessage, textAlertDTO.DlrStatus, textAlertDTO.Status, textAlertDTO.StatusDescription, textAlertDTO.Reference, textAlertDTO.Origin, textAlertDTO.Priority, textAlertDTO.SendRetry, textAlertDTO.RecordStatus, serviceHeader);

      var result = await _repository!.AddAsync(aggregateToAdd, cts.Token);
      await _unitOfWork.SaveAsync();
      var domainEvent = new TextAlertAddedEvent(result);
      await _mediator.Publish(domainEvent, cts.Token).ConfigureAwait(false);
      return Result.Success(_unitOfWork.MapTo<TextAlertDTO>(result));
    }
    catch (Exception ex)
    {
      // TODO: Log details here
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result<bool>> BulkInsertTextAlertsAsync(List<TextAlertDTO> textAlertDTOs, ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    var result = default(bool);

    try
    {
      if (_readRepository!.IsDatabaseSqlServer())
      {
        var textAlerts = new List<TextAlert>();

        textAlertDTOs.ForEach(textAlertDTO =>
        {
          var textMessage = new TextMessage(textAlertDTO.TextMessageRecipient!, textAlertDTO.TextMessageBody!, textAlertDTO.TextMessageSecurityCritical);

          textAlerts.Add(TextAlertFactory.CreateTextAlert(textMessage, textAlertDTO.DlrStatus, textAlertDTO.Status, textAlertDTO.StatusDescription, textAlertDTO.Reference, textAlertDTO.Origin, textAlertDTO.Priority, textAlertDTO.SendRetry, textAlertDTO.RecordStatus, serviceHeader));
        });

        result = await _repository!.DatabaseBulkInsertAsync(textAlerts, Utility.DbTableName<TextAlert>(), serviceHeader, cts.Token);

        result = await _unitOfWork.SaveAsync();
      }
      else
      {
        textAlertDTOs.ForEach(async textAlertDTO =>
        {
          result = await AddTextAlertAsync(textAlertDTO, serviceHeader, cts.Token) != null;
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

  public async Task<Result> UpdateTextAlertAsync(Guid textAlertId, TextAlertDTO textAlertDTO, ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      var aggregateToUpdate = await _repository!.GetByIdAsync(textAlertId, cts.Token);
      if (aggregateToUpdate == null) return Result.NotFound();

      var textMessage = new TextMessage(textAlertDTO.TextMessageRecipient!, textAlertDTO.TextMessageBody!, textAlertDTO.TextMessageSecurityCritical);

      aggregateToUpdate = TextAlertFactory.CreateTextAlert(textMessage, textAlertDTO.DlrStatus, textAlertDTO.Status, textAlertDTO.StatusDescription, textAlertDTO.Reference, textAlertDTO.Origin, textAlertDTO.Priority, textAlertDTO.SendRetry, textAlertDTO.RecordStatus, serviceHeader, aggregateToUpdate);

      await _repository!.UpdateAsync(aggregateToUpdate, cts.Token);
      await _unitOfWork.SaveAsync();
      var domainEvent = new TextAlertUpdatedEvent(aggregateToUpdate);
      await _mediator.Publish(domainEvent, cts.Token).ConfigureAwait(false);
      return Result.Success();
    }
    catch (Exception ex)
    {
      // TODO: Log details here
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result> DeleteTextAlertAsync(Guid textAlertId, ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      var aggregateToDelete = await _readRepository!.GetByIdAsync(textAlertId, cts.Token);
      if (aggregateToDelete == null) return Result.NotFound();

      await _repository!.DeleteAsync(aggregateToDelete, cts.Token);
      var domainEvent = new TextAlertDeletedEvent(textAlertId);
      await _mediator.Publish(domainEvent, cts.Token).ConfigureAwait(false);
      return Result.Success();
    }
    catch (Exception ex)
    {
      // TODO: Log details here
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result<TextAlertDTO>> FindTextAlertAsync(Guid textAlertId, ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      var aggregate = await _readRepository!.GetByIdAsync(textAlertId, cts.Token);

      if (aggregate == null)
      {
        return Result.NotFound();
      }

      return new Result<TextAlertDTO>(_unitOfWork.MapTo<TextAlertDTO>(aggregate));
    }
    catch (Exception ex)
    {
      // TODO: Log details here
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result<List<TextAlertDTO>>> FindTextAlertsAsync(ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      var textAlertDTOs = await _readRepository!.ListAsync<TextAlertDTO>(cts.Token);

      if (textAlertDTOs == null)
      {
        return Result.NotFound();
      }

      return new Result<List<TextAlertDTO>>(textAlertDTOs);
    }
    catch (Exception ex)
    {
      // TODO: Log details here
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result<PageCollectionInfo<TextAlertDTO>>> GetTextAlertsWithFiltersAndInPageAsync(string? searchString, int pageNumber, int pageSize, string sortColumn, string sortDirection, ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      var allTextAlertSpec = new TextAlertsWithFiltersAndInPageSpec(searchString, sortColumn, sortDirection);

      var textAlerts = await PaginatedIAggregateRootList<TextAlert, TextAlertDTO>.CreatePageCollectionInfoAsync(_unitOfWork, allTextAlertSpec, pageNumber, pageSize, _unitOfWork.MapTo<TextAlertDTO>, cts.Token);

      if (textAlerts != null)
      {
        if (textAlerts.PageCollection == null)
        {
          return Result<PageCollectionInfo<TextAlertDTO>>.NotFound();
        }

        return new Result<PageCollectionInfo<TextAlertDTO>>(textAlerts);
      }

      return Result<PageCollectionInfo<TextAlertDTO>>.NotFound("");
    }
    catch (Exception ex)
    {
      // TODO: Log details here
      return Result<PageCollectionInfo<TextAlertDTO>>.Error(new[] { ex.Message });
    }
  }
}

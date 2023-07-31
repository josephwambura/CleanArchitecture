using Ardalis.Result;

using Clean.Architecture.Application.Core.Interfaces.AdministrationModule;
using Clean.Architecture.Application.DTO.AdministrationModule.EnumerationAggregate;
using Clean.Architecture.Core.AdministrationModule.EnumerationAggregate;
using Clean.Architecture.Core.AdministrationModule.EnumerationAggregate.Events;
using Clean.Architecture.Core.AdministrationModule.EnumerationAggregate.Specifications;
using Clean.Architecture.SharedKernel;
using Clean.Architecture.SharedKernel.Interfaces;
using Clean.Architecture.SharedKernel.Utils;

using MediatR;

namespace Clean.Architecture.Application.Core.Services.AdministrationModule;

public class EnumerationAppService : IEnumerationAppService
{
  private readonly IUnitOfWork _unitOfWork;
  private readonly IReadRepository<Enumeration>? _readRepository;
  private readonly IRepository<Enumeration>? _repository;
  private readonly IMediator _mediator;

  public EnumerationAppService(IUnitOfWork unitOfWork, IMediator mediator)
  {
    _unitOfWork = unitOfWork;
    _mediator = mediator;

    _readRepository = _unitOfWork.GetIReadRepository<Enumeration>((byte)DbConnectionContext.LIVE);
    _repository = _unitOfWork.GetIRepository<Enumeration>((byte)DbConnectionContext.LIVE);
  }

  public async Task<Result<EnumerationDTO>> AddEnumerationAsync(EnumerationDTO enumerationDTO, ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      var aggregateToAdd = EnumerationFactory.CreateEnumeration(enumerationDTO.Key!, enumerationDTO.Value, enumerationDTO.Description!, enumerationDTO.RecordStatus, serviceHeader);

      var result = await _repository!.AddAsync(aggregateToAdd, cts.Token);
      await _unitOfWork.SaveAsync();
      var domainEvent = new EnumerationAddedEvent(result);
      await _mediator.Publish(domainEvent, cts.Token).ConfigureAwait(false);
      return Result.Success(_unitOfWork.MapTo<EnumerationDTO>(result));
    }
    catch (Exception ex)
    {
      // TODO: Log details here
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result<bool>> BulkInsertEnumerationsAsync(List<EnumerationDTO> enumerationDTOs, ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    var result = default(bool);

    try
    {
      if (_readRepository!.IsDatabaseSqlServer())
      {
        var enumerations = new List<Enumeration>();

        enumerationDTOs.ForEach(enumerationDTO =>
        {
          enumerations.Add(EnumerationFactory.CreateEnumeration(enumerationDTO.Key!, enumerationDTO.Value, enumerationDTO.Description!, enumerationDTO.RecordStatus, serviceHeader));
        });

        result = await _repository!.DatabaseBulkInsertAsync(enumerations, Utility.DbTableName<Enumeration>(), serviceHeader, cts.Token);

        result = await _unitOfWork.SaveAsync();
      }
      else
      {
        enumerationDTOs.ForEach(async enumerationDTO =>
        {
          result = await AddEnumerationAsync(enumerationDTO, serviceHeader, cts.Token) != null;
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

  public async Task<Result> UpdateEnumerationAsync(Guid enumerationId, EnumerationDTO enumerationDTO, ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      var aggregateToUpdate = await _repository!.GetByIdAsync(enumerationId, cts.Token);
      if (aggregateToUpdate == null) return Result.NotFound();

      aggregateToUpdate = EnumerationFactory.CreateEnumeration(enumerationDTO.Key!, enumerationDTO.Value, enumerationDTO.Description!, enumerationDTO.RecordStatus, serviceHeader, aggregateToUpdate);

      await _repository!.UpdateAsync(aggregateToUpdate, cts.Token);
      await _unitOfWork.SaveAsync();
      var domainEvent = new EnumerationUpdatedEvent(aggregateToUpdate);
      await _mediator.Publish(domainEvent, cts.Token).ConfigureAwait(false);
      return Result.Success();
    }
    catch (Exception ex)
    {
      // TODO: Log details here
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result> DeleteEnumerationAsync(Guid enumerationId, ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      var aggregateToDelete = await _readRepository!.GetByIdAsync(enumerationId, cts.Token);
      if (aggregateToDelete == null) return Result.NotFound();

      await _repository!.DeleteAsync(aggregateToDelete, cts.Token);
      var domainEvent = new EnumerationDeletedEvent(enumerationId);
      await _mediator.Publish(domainEvent, cts.Token).ConfigureAwait(false);
      return Result.Success();
    }
    catch (Exception ex)
    {
      // TODO: Log details here
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result<EnumerationDTO>> FindEnumerationAsync(Guid enumerationId, ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      var aggregate = await _readRepository!.GetByIdAsync(enumerationId, cts.Token);

      if (aggregate == null)
      {
        return Result.NotFound();
      }

      return new Result<EnumerationDTO>(_unitOfWork.MapTo<EnumerationDTO>(aggregate));
    }
    catch (Exception ex)
    {
      // TODO: Log details here
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result<List<EnumerationDTO>>> FindEnumerationsAsync(ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      var enumerationDTOs = await _readRepository!.ListAsync<EnumerationDTO>(cts.Token);

      if (enumerationDTOs == null)
      {
        return Result.NotFound();
      }

      return new Result<List<EnumerationDTO>>(enumerationDTOs);
    }
    catch (Exception ex)
    {
      // TODO: Log details here
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result<PageCollectionInfo<EnumerationDTO>>> GetEnumerationsWithFiltersAndInPageAsync(string? searchString, int pageNumber, int pageSize, string sortColumn, string sortDirection, ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      var allEnumerationSpec = new EnumerationsWithFiltersAndInPageSpec(searchString, sortColumn, sortDirection);

      var enumerations = await PaginatedIAggregateRootList<Enumeration, EnumerationDTO>.CreatePageCollectionInfoAsync(_unitOfWork, allEnumerationSpec, pageNumber, pageSize, _unitOfWork.MapTo<EnumerationDTO>, cts.Token);

      if (enumerations != null)
      {
        if (enumerations.PageCollection == null)
        {
          return Result<PageCollectionInfo<EnumerationDTO>>.NotFound();
        }

        return new Result<PageCollectionInfo<EnumerationDTO>>(enumerations);
      }

      return Result<PageCollectionInfo<EnumerationDTO>>.NotFound("");
    }
    catch (Exception ex)
    {
      // TODO: Log details here
      return Result<PageCollectionInfo<EnumerationDTO>>.Error(new[] { ex.Message });
    }
  }
}

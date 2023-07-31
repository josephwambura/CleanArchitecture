using Ardalis.Result;

using Clean.Architecture.Application.DTO.AdministrationModule.StaticSettingAggregate;
using Clean.Architecture.Application.Interfaces.AdministrationModule;
using Clean.Architecture.Core.AdministrationModule.StaticSettingAggregate;
using Clean.Architecture.Core.AdministrationModule.StaticSettingAggregate.Events;
using Clean.Architecture.Core.AdministrationModule.StaticSettingAggregate.Specifications;
using Clean.Architecture.SharedKernel;
using Clean.Architecture.SharedKernel.Interfaces;
using Clean.Architecture.SharedKernel.Utils;

using MediatR;

namespace Clean.Architecture.Application.Services.AdministrationModule;

public class StaticSettingAppService : IStaticSettingAppService
{
  private readonly IUnitOfWork _unitOfWork;
  private readonly IReadRepository<StaticSetting>? _readRepository;
  private readonly IRepository<StaticSetting>? _repository;
  private readonly IMediator _mediator;

  public StaticSettingAppService(IUnitOfWork unitOfWork, IMediator mediator)
  {
    _unitOfWork = unitOfWork;
    _mediator = mediator;

    _readRepository = _unitOfWork.GetIReadRepository<StaticSetting>((byte)DbConnectionContext.LIVE);
    _repository = _unitOfWork.GetIRepository<StaticSetting>((byte)DbConnectionContext.LIVE);
  }

  public async Task<Result<StaticSettingDTO>> AddStaticSettingAsync(StaticSettingDTO staticSettingDTO, ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      var aggregateToAdd = StaticSettingFactory.CreateStaticSetting(staticSettingDTO.Key, staticSettingDTO.Value, staticSettingDTO.IsActive, staticSettingDTO.SecurityCritical, (byte)RecordStatus.Approved, serviceHeader);

      var result = await _repository!.AddAsync(aggregateToAdd, cts.Token);
      await _unitOfWork.SaveAsync();
      var domainEvent = new StaticSettingAddedEvent(result);
      await _mediator.Publish(domainEvent, cts.Token).ConfigureAwait(false);
      return Result.Success(_unitOfWork.MapTo<StaticSettingDTO>(result));
    }
    catch (Exception ex)
    {
      // TODO: Log details here
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result> UpdateStaticSettingAsync(Guid staticSettingId, StaticSettingDTO staticSettingDTO, ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      var aggregateToUpdate = await _repository!.GetByIdAsync(staticSettingId, cts.Token);
      if (aggregateToUpdate == null) return Result.NotFound();

      aggregateToUpdate = StaticSettingFactory.CreateStaticSetting(staticSettingDTO.Key, staticSettingDTO.Value, staticSettingDTO.IsActive, staticSettingDTO.SecurityCritical, (byte)RecordStatus.Approved, serviceHeader, aggregateToUpdate);

      await _repository!.UpdateAsync(aggregateToUpdate, cts.Token);
      await _unitOfWork.SaveAsync();
      var domainEvent = new StaticSettingUpdatedEvent(aggregateToUpdate);
      await _mediator.Publish(domainEvent, cts.Token).ConfigureAwait(false);
      return Result.Success();
    }
    catch (Exception ex)
    {
      // TODO: Log details here
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result> DeleteStaticSettingAsync(Guid staticSettingId, ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      var aggregateToDelete = await _readRepository!.GetByIdAsync(staticSettingId, cts.Token);
      if (aggregateToDelete == null) return Result.NotFound();

      await _repository!.DeleteAsync(aggregateToDelete, cts.Token);
      var domainEvent = new StaticSettingDeletedEvent(staticSettingId);
      await _mediator.Publish(domainEvent, cts.Token).ConfigureAwait(false);
      return Result.Success();
    }
    catch (Exception ex)
    {
      // TODO: Log details here
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result<StaticSettingDTO>> FindStaticSettingAsync(Guid staticSettingId, ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      var aggregate = await _readRepository!.GetByIdAsync(staticSettingId, cts.Token);

      if (aggregate == null)
      {
        return Result.NotFound();
      }

      return new Result<StaticSettingDTO>(_unitOfWork.MapTo<StaticSettingDTO>(aggregate));
    }
    catch (Exception ex)
    {
      // TODO: Log details here
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result<StaticSettingDTO>> FindStaticSettingByKeyAsync(string key, ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      var spec = new StaticSettingByKeySpec(key);

      var aggregate = await _readRepository!.FirstOrDefaultAsync(spec, cts.Token);

      if (aggregate == null)
      {
        return Result.NotFound();
      }

      return new Result<StaticSettingDTO>(_unitOfWork.MapTo<StaticSettingDTO>(aggregate));
    }
    catch (Exception ex)
    {
      // TODO: Log details here
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result<List<StaticSettingDTO>>> FindStaticSettingsAsync(ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      var staticSettingDTOs = await _readRepository!.ListAsync<StaticSettingDTO>(cts.Token);

      if (staticSettingDTOs == null)
      {
        return Result.NotFound();
      }

      return new Result<List<StaticSettingDTO>>(staticSettingDTOs);
    }
    catch (Exception ex)
    {
      // TODO: Log details here
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result<PageCollectionInfo<StaticSettingDTO>>> GetStaticSettingsWithFiltersAndInPageAsync(string? searchString, int pageNumber, int pageSize, string sortColumn, string sortDirection, ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      var allStaticSettingSpec = new StaticSettingsWithFiltersAndInPageSpec(searchString, sortColumn, sortDirection);

      var staticSettings = await PaginatedIAggregateRootList<StaticSetting, StaticSettingDTO>.CreatePageCollectionInfoAsync(_unitOfWork, allStaticSettingSpec, pageNumber, pageSize, _unitOfWork.MapTo<StaticSettingDTO>, cts.Token);

      if (staticSettings != null)
      {
        if (staticSettings.PageCollection == null)
        {
          return Result<PageCollectionInfo<StaticSettingDTO>>.NotFound();
        }

        return new Result<PageCollectionInfo<StaticSettingDTO>>(staticSettings);
      }

      return Result<PageCollectionInfo<StaticSettingDTO>>.NotFound("");
    }
    catch (Exception ex)
    {
      // TODO: Log details here
      return Result<PageCollectionInfo<StaticSettingDTO>>.Error(new[] { ex.Message });
    }
  }
}

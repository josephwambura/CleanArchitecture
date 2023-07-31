using Ardalis.Result;

using Clean.Architecture.Application.DTO.WeatherForecastAggregate;
using Clean.Architecture.Application.Interfaces;
using Clean.Architecture.Core.WeatherForecastAggregate;
using Clean.Architecture.Core.WeatherForecastAggregate.Events;
using Clean.Architecture.Core.WeatherForecastAggregate.Specifications;
using Clean.Architecture.SharedKernel;
using Clean.Architecture.SharedKernel.Extensions;
using Clean.Architecture.SharedKernel.Interfaces;
using Clean.Architecture.SharedKernel.Utils;

using MediatR;

namespace Clean.Architecture.Application.Services;

public class WeatherForecastService : IWeatherForecastService
{
  private readonly IUnitOfWork _unitOfWork;
  private readonly IReadRepository<WeatherForecast>? _readRepository;
  private readonly IRepository<WeatherForecast>? _repository;
  private readonly IMediator _mediator;

  public WeatherForecastService(IUnitOfWork unitOfWork, IMediator mediator)
  {
    _unitOfWork = unitOfWork;
    _mediator = mediator;

    _readRepository = _unitOfWork.GetIReadRepository<WeatherForecast>((byte)DbConnectionContext.LIVE);
    _repository = _unitOfWork.GetIRepository<WeatherForecast>((byte)DbConnectionContext.LIVE);
  }

  public async Task<Result<WeatherForecastDTO>> AddWeatherForecastAsync(WeatherForecastDTO weatherForecastDTO, ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      var aggregateToAdd = WeatherForecastFactory.CreateWeatherForecast(weatherForecastDTO.Date, weatherForecastDTO.TemperatureC, weatherForecastDTO.Summary!, (byte)RecordStatus.Approved, serviceHeader);

      var result = await _repository!.AddAsync(aggregateToAdd, cts.Token);
      await _unitOfWork.SaveAsync();
      var domainEvent = new WeatherForecastAddedEvent(result);
      await _mediator.Publish(domainEvent, cts.Token).ConfigureAwait(false);
      return Result.Success(_unitOfWork.MapTo<WeatherForecastDTO>(result));
    }
    catch (Exception ex)
    {
      // TODO: Log details here
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result> UpdateWeatherForecastAsync(Guid weatherForecastId, WeatherForecastDTO weatherForecastDTO, ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      var aggregateToUpdate = await _repository!.GetByIdAsync(weatherForecastId, cts.Token);
      if (aggregateToUpdate == null) return Result.NotFound();

      aggregateToUpdate = WeatherForecastFactory.CreateWeatherForecast(weatherForecastDTO.Date, weatherForecastDTO.TemperatureC, weatherForecastDTO.Summary!, (byte)RecordStatus.Approved, serviceHeader, aggregateToUpdate);

      await _repository!.UpdateAsync(aggregateToUpdate, cts.Token);
      await _unitOfWork.SaveAsync();
      var domainEvent = new WeatherForecastUpdatedEvent(aggregateToUpdate);
      await _mediator.Publish(domainEvent, cts.Token).ConfigureAwait(false);
      return Result.Success();
    }
    catch (Exception ex)
    {
      // TODO: Log details here
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result> DeleteWeatherForecastAsync(Guid weatherForecastId, ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      var aggregateSpec = new WeatherForecastByIdSpec(weatherForecastId);
      var aggregateToDelete = await _repository!.FirstOrDefaultAsync(aggregateSpec, cts.Token);
      if (aggregateToDelete == null) return Result.NotFound();

      await _repository!.DeleteAsync(aggregateToDelete, cts.Token);
      var domainEvent = new WeatherForecastDeletedEvent(weatherForecastId);
      await _mediator.Publish(domainEvent, cts.Token).ConfigureAwait(false);
      return Result.Success();
    }
    catch (Exception ex)
    {
      // TODO: Log details here
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result<WeatherForecastDTO>> FindWeatherForecastAsync(Guid weatherForecastId, ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      var aggregateSpec = new WeatherForecastByIdSpec(weatherForecastId);
      var weatherForecastDTO = await _readRepository!.FirstOrDefaultAsync<WeatherForecastDTO>(aggregateSpec, cts.Token);

      if (weatherForecastDTO == null)
      {
        return Result.NotFound();
      }

      return new Result<WeatherForecastDTO>(weatherForecastDTO);
    }
    catch (Exception ex)
    {
      // TODO: Log details here
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result<List<WeatherForecastDTO>>> FindWeatherForecastsAsync(ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      var weatherForecastDTOs = await _readRepository!.ListAsync<WeatherForecastDTO>(cts.Token);

      if (weatherForecastDTOs == null)
      {
        return Result.NotFound();
      }

      return new Result<List<WeatherForecastDTO>>(weatherForecastDTOs);
    }
    catch (Exception ex)
    {
      // TODO: Log details here
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result<PageCollectionInfo<WeatherForecastDTO>>> GetWeatherForecastsWithFiltersAndInPageAsync(string? searchString, int pageNumber, int pageSize, string sortColumn, string sortDirection, ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      var allWeatherForecastSpec = new WeatherForecastsWithFiltersAndInPageSpec(searchString, sortColumn, sortDirection);

      var weatherForecasts = await PaginatedIAggregateRootList<WeatherForecast, WeatherForecastDTO>.CreatePageCollectionInfoAsync(_unitOfWork, allWeatherForecastSpec, pageNumber, pageSize, _unitOfWork.MapTo<WeatherForecastDTO>, cts.Token);

      if (weatherForecasts != null)
      {
        if (weatherForecasts.PageCollection == null)
        {
          return Result<PageCollectionInfo<WeatherForecastDTO>>.NotFound();
        }

        return new Result<PageCollectionInfo<WeatherForecastDTO>>(weatherForecasts);
      }

      return Result<PageCollectionInfo<WeatherForecastDTO>>.NotFound("");
    }
    catch (Exception ex)
    {
      // TODO: Log details here
      return Result<PageCollectionInfo<WeatherForecastDTO>>.Error(new[] { ex.Message });
    }
  }
}

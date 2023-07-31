using Ardalis.Result;

using Clean.Architecture.Application.DTO.WeatherForecastAggregate;
using Clean.Architecture.SharedKernel;
using Clean.Architecture.SharedKernel.Utils;

namespace Clean.Architecture.Application.Interfaces;

public interface IWeatherForecastService
{
  Task<Result<WeatherForecastDTO>> AddWeatherForecastAsync(WeatherForecastDTO weatherForecastDTO, ServiceHeader serviceHeader, CancellationToken cancellationToken = default);

  Task<Result> UpdateWeatherForecastAsync(Guid weatherForecastId, WeatherForecastDTO weatherForecastDTO, ServiceHeader serviceHeader, CancellationToken cancellationToken = default);

  Task<Result> DeleteWeatherForecastAsync(Guid weatherForecastId, ServiceHeader serviceHeader, CancellationToken cancellationToken = default);

  Task<Result<WeatherForecastDTO>> FindWeatherForecastAsync(Guid weatherForecastId, ServiceHeader serviceHeader, CancellationToken cancellationToken = default);

  Task<Result<List<WeatherForecastDTO>>> FindWeatherForecastsAsync(ServiceHeader serviceHeader, CancellationToken cancellationToken = default);

  Task<Result<PageCollectionInfo<WeatherForecastDTO>>> GetWeatherForecastsWithFiltersAndInPageAsync(string? searchString, int pageNumber, int pageSize, string sortColumn, string sortDirection, ServiceHeader serviceHeader, CancellationToken cancellationToken = default);
}

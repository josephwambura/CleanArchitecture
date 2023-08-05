using Ardalis.Specification;

using Clean.Architecture.Domain.WeatherForecastAggregate;

namespace Clean.Architecture.Domain.WeatherForecastAggregate.Specifications;

public class WeatherForecastByIdSpec : Specification<WeatherForecast>, ISingleResultSpecification<WeatherForecast>
{
  public WeatherForecastByIdSpec(Guid weatherForecastId)
  {
    Query
        .Where(weatherForecast => weatherForecast.Id == weatherForecastId);
  }
}

using Ardalis.Specification;

namespace Clean.Architecture.Core.WeatherForecastAggregate.Specifications;

public class WeatherForecastByIdSpec : Specification<WeatherForecast>, ISingleResultSpecification<WeatherForecast>
{
  public WeatherForecastByIdSpec(Guid weatherForecastId)
  {
    Query
        .Where(weatherForecast => weatherForecast.Id == weatherForecastId);
  }
}

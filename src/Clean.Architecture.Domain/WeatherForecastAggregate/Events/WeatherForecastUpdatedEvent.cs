using Clean.Architecture.Domain.WeatherForecastAggregate;
using Clean.Architecture.SharedKernel;

namespace Clean.Architecture.Domain.WeatherForecastAggregate.Events;

public class WeatherForecastUpdatedEvent : DomainEventBase
{
  public WeatherForecast WeatherForecast { get; set; }

  public WeatherForecastUpdatedEvent(WeatherForecast project)
  {
    WeatherForecast = project;
  }
}

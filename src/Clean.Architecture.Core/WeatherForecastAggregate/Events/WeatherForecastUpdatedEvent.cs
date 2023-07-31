using Clean.Architecture.SharedKernel;

namespace Clean.Architecture.Core.WeatherForecastAggregate.Events;

public class WeatherForecastUpdatedEvent : DomainEventBase
{
  public WeatherForecast WeatherForecast { get; set; }

  public WeatherForecastUpdatedEvent(WeatherForecast project)
  {
    WeatherForecast = project;
  }
}

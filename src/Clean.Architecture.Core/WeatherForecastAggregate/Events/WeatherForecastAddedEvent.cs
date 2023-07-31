using Clean.Architecture.SharedKernel;

namespace Clean.Architecture.Core.WeatherForecastAggregate.Events;

public class WeatherForecastAddedEvent : DomainEventBase
{
  public WeatherForecast WeatherForecast { get; set; }

  public WeatherForecastAddedEvent(WeatherForecast project)
  {
    WeatherForecast = project;
  }
}

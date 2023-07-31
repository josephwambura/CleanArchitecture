using Clean.Architecture.SharedKernel.Extensions;
using Clean.Architecture.SharedKernel.Utils;

namespace Clean.Architecture.Core.WeatherForecastAggregate;

public static class WeatherForecastFactory
{
  public static WeatherForecast CreateWeatherForecast(DateTimeOffset date, int temperatureC, string summary, byte recordStatus, ServiceHeader serviceHeader, WeatherForecast? original = null)
  {
    var entity = original ?? new WeatherForecast()
    {
      Date = date,
      TemperatureC = temperatureC,
      Summary = summary,
    };

    EntityFactoryExtensions.BaseEntityDetails(entity, serviceHeader, original);

    return entity;
  }
}

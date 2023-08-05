using Clean.Architecture.SharedKernel.Interfaces;
using Clean.Architecture.SharedKernel;

namespace Clean.Architecture.Domain.WeatherForecastAggregate;

public class WeatherForecast : EntityBase, IAggregateRoot
{
  public virtual DateTimeOffset Date { get; set; }

  public virtual DateOnly DateOnly => DateOnly.FromDateTime(Date.UtcDateTime);

  public virtual int TemperatureC { get; set; }

  public virtual int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

  public virtual string? Summary { get; set; }
}

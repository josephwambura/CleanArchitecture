using Clean.Architecture.SharedKernel.Extensions;
using Clean.Architecture.SharedKernel.Utils;

namespace Clean.Architecture.Application.DTO.WeatherForecastAggregate;

public class WeatherForecastDTO
{
  public Guid Id { get; set; }

  public virtual DateTimeOffset Date { get; set; }

  public virtual DateOnly DateOnly => DateOnly.FromDateTime(Date.UtcDateTime);

  public virtual int TemperatureC { get; set; }

  public virtual int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

  public virtual string? Summary { get; set; }

  #region Common Properties

  public DateTimeOffset CreatedDate { get; set; }
  public string? FormattedCreatedDate => CreatedDate.ISO8601DateTimeFormat();
  public DateTimeOffset? ModifiedDate { get; set; }
  public string? FormattedModifiedDate => ModifiedDate?.ISO8601DateTimeFormat();
  public string CreatedBy { get; set; } = default!;
  public string? ModifiedBy { get; set; }
  public byte RecordStatus { get; set; }
  public string? RecordStatusDescription => ((RecordStatus)RecordStatus).GetDescription();

  #endregion
}

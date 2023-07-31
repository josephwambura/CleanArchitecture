using Clean.Architecture.Core.AdministrationModule.StaticSettingAggregate;
using Clean.Architecture.SharedKernel.Extensions;
using Clean.Architecture.SharedKernel.Utils;

namespace Clean.Architecture.Application.DTO.AdministrationModule.StaticSettingAggregate;

public class StaticSettingDTO
{
  public Guid Id { get; set; }
  public string Key { get; set; } = string.Empty;
  public string Value { get; set; } = string.Empty;
  public bool IsActive { get; set; }
  public bool SecurityCritical { get; set; }

  #region Common Properties

  public DateTime CreatedDate { get; set; }
  public string? FormattedCreatedDate => CreatedDate.ISO8601DateTimeFormat();
  public DateTime? ModifiedDate { get; set; }
  public string? FormattedModifiedDate => ModifiedDate?.ISO8601DateTimeFormat();
  public string CreatedBy { get; set; } = default!;
  public string? ModifiedBy { get; set; }
  public byte RecordStatus { get; set; }

  #endregion
}

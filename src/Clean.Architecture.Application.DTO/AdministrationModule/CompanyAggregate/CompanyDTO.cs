using Clean.Architecture.SharedKernel.Extensions;

namespace Clean.Architecture.Application.DTO.AdministrationModule.CompanyAggregate;

public class CompanyDTO
{
  public Guid Id { get; set; }
  public string Name { get; set; } = string.Empty;

  #region Common Properties

  public DateTimeOffset CreatedDate { get; set; }
  public string? FormattedCreatedDate => CreatedDate.ISO8601DateTimeFormat();
  public DateTimeOffset? ModifiedDate { get; set; }
  public string? FormattedModifiedDate => ModifiedDate?.ISO8601DateTimeFormat();
  public string CreatedBy { get; set; } = default!;
  public string? ModifiedBy { get; set; }
  public byte RecordStatus { get; set; }

  #endregion
}

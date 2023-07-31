using Clean.Architecture.Core.ContributorAggregate;
using Clean.Architecture.SharedKernel.Extensions;

namespace Clean.Architecture.Application.DTO.ContributorAggregate;

public class ContributorDTO
{
  public Guid Id { get; set; }
  public string Name { get; set; } = string.Empty;

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

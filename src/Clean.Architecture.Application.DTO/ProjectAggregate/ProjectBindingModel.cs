using Clean.Architecture.Core.ProjectAggregate;
using Clean.Architecture.SharedKernel.Extensions;
using Clean.Architecture.SharedKernel.Utils;

namespace Clean.Architecture.Application.DTO.ProjectAggregate;

public class ProjectBindingModel
{
  public Guid Id { get; set; }
  public string? Name { get; set; }
  public List<ToDoItemViewModel> Items = new();
  public int? PriorityValue { get; set; }
  public string? PriorityName { get; set; }

  #region Common Properties

  public DateTime CreatedDate { get; set; }
  public string? FormattedCreatedDate => CreatedDate.ISO8601DateTimeFormat();
  public DateTime? ModifiedDate { get; set; }
  public string? FormattedModifiedDate => ModifiedDate?.ISO8601DateTimeFormat();
  public string CreatedBy { get; set; } = default!;
  public string? ModifiedBy { get; set; }
  public byte RecordStatus { get; set; }
  public string? RecordStatusDescription => ((RecordStatus)RecordStatus).GetDescription();

  #endregion
}

using Clean.Architecture.Core.ProjectAggregate;
using Clean.Architecture.SharedKernel.Extensions;
using Clean.Architecture.SharedKernel.Utils;

namespace Clean.Architecture.Application.DTO.ProjectAggregate;

public class ToDoItemDTO
{
  public Guid Id { get; set; }
  public string Title { get; set; } = string.Empty;
  public string Description { get; set; } = string.Empty;
  public Guid? ContributorId { get; set; }
  public bool IsDone { get; set; }

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

  public static ToDoItemDTO FromToDoItem(ToDoItem item)
  {
    return new ToDoItemDTO()
    {
      Id = item.Id,
      Title = item.Title,
      Description = item.Description,
      IsDone = item.IsDone,
      ContributorId = item.ContributorId,

      #region Common Properties

      CreatedBy = item.CreatedBy,
      CreatedDate = item.CreatedDate,
      ModifiedBy = item.ModifiedBy,
      ModifiedDate = item.ModifiedDate,
      RecordStatus = item.RecordStatus

      #endregion
    };
  }
}

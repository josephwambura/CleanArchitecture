﻿using Clean.Architecture.SharedKernel.Extensions;
using Clean.Architecture.SharedKernel.Utils;

namespace Clean.Architecture.Application.DTO.ProjectAggregate;

public class ProjectDTO
{
  public Guid Id { get; set; }
  public string Name { get; set; } = string.Empty;

  public List<ToDoItemDTO>? Items { get; set; }
  public ProjectStatus Status => Items != null & Items!.All(i => i.IsDone) ? ProjectStatus.Complete : ProjectStatus.InProgress;
  public string? StatusDescription => Status.GetDescription();

  public PriorityStatus Priority { get; set; } = PriorityStatus.Backlog;

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
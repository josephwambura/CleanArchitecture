using Clean.Architecture.SharedKernel;

namespace Clean.Architecture.Core.ProjectAggregate.Events;

public class ProjectDeletedEvent : DomainEventBase
{
  public Guid ProjectId { get; set; }

  public ProjectDeletedEvent(Guid projectId)
  {
    ProjectId = projectId;
  }
}

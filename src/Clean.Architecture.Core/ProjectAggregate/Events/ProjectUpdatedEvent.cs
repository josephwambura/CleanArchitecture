using Clean.Architecture.SharedKernel;

namespace Clean.Architecture.Core.ProjectAggregate.Events;

public class ProjectUpdatedEvent : DomainEventBase
{
  public Project Project { get; set; }

  public ProjectUpdatedEvent(Project project)
  {
    Project = project;
  }
}

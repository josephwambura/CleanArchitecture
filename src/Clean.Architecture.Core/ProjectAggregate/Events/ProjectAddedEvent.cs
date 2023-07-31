using Clean.Architecture.SharedKernel;

namespace Clean.Architecture.Core.ProjectAggregate.Events;

public class ProjectAddedEvent : DomainEventBase
{
  public Project Project { get; set; }

  public ProjectAddedEvent(Project project)
  {
    Project = project;
  }
}

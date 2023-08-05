using Clean.Architecture.Domain.ProjectAggregate;
using Clean.Architecture.SharedKernel;

namespace Clean.Architecture.Domain.ProjectAggregate.Events;

public class ProjectAddedEvent : DomainEventBase
{
  public Project Project { get; set; }

  public ProjectAddedEvent(Project project)
  {
    Project = project;
  }
}

using Clean.Architecture.Domain.ProjectAggregate;
using Clean.Architecture.SharedKernel;

namespace Clean.Architecture.Domain.ProjectAggregate.Events;

public class ProjectUpdatedEvent : DomainEventBase
{
  public Project Project { get; set; }

  public ProjectUpdatedEvent(Project project)
  {
    Project = project;
  }
}

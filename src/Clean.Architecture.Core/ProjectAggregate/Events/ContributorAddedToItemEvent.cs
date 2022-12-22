using Clean.Architecture.Core.ContributorAggregate;
using Clean.Architecture.SharedKernel;

namespace Clean.Architecture.Core.ProjectAggregate.Events;

public class ContributorAddedToItemEvent : DomainEventBase
{
  public Guid ContributorId { get; set; }
  public ToDoItem Item { get; set; }

  public ContributorAddedToItemEvent(ToDoItem item, Guid contributorId)
  {
    Item = item;
    ContributorId = contributorId;
  }
}

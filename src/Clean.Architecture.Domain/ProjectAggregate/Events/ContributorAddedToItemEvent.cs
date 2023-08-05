using Clean.Architecture.SharedKernel;

namespace Clean.Architecture.Domain.ProjectAggregate.Events;

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

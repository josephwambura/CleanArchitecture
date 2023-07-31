using Clean.Architecture.SharedKernel;

namespace Clean.Architecture.Core.ContributorAggregate.Events;

public class ContributorDeletedEvent : DomainEventBase
{
  public Guid ContributorId { get; set; }

  public ContributorDeletedEvent(Guid contributorId)
  {
    ContributorId = contributorId;
  }
}

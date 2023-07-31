using Clean.Architecture.SharedKernel;

namespace Clean.Architecture.Core.AdministrationModule.EnumerationAggregate.Events;

public class EnumerationDeletedEvent : DomainEventBase
{
  public Guid EnumerationId { get; set; }

  public EnumerationDeletedEvent(Guid enumerationId)
  {
    EnumerationId = enumerationId;
  }
}

using Clean.Architecture.SharedKernel;

namespace Clean.Architecture.Domain.AdministrationModule.EnumerationAggregate.Events;

public class EnumerationUpdatedEvent : DomainEventBase
{
  public Enumeration Enumeration { get; set; }

  public EnumerationUpdatedEvent(Enumeration enumeration)
  {
    Enumeration = enumeration;
  }
}

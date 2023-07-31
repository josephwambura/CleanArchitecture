using Clean.Architecture.SharedKernel;

namespace Clean.Architecture.Core.AdministrationModule.EnumerationAggregate.Events;

public class EnumerationAddedEvent : DomainEventBase
{
  public Enumeration Enumeration { get; set; }

  public EnumerationAddedEvent(Enumeration enumeration)
  {
    Enumeration = enumeration;
  }
}

using Clean.Architecture.SharedKernel;

namespace Clean.Architecture.Domain.AdministrationModule.HolidayAggregate.Events;

public class HolidayUpdatedEvent : DomainEventBase
{
  public Holiday Holiday { get; set; }

  public HolidayUpdatedEvent(Holiday holiday)
  {
    Holiday = holiday;
  }
}

using Clean.Architecture.SharedKernel;

namespace Clean.Architecture.Domain.AdministrationModule.HolidayAggregate.Events;

public class HolidayAddedEvent : DomainEventBase
{
  public Holiday Holiday { get; set; }

  public HolidayAddedEvent(Holiday holiday)
  {
    Holiday = holiday;
  }
}

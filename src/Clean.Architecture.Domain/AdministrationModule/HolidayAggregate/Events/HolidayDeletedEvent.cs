using Clean.Architecture.SharedKernel;

namespace Clean.Architecture.Domain.AdministrationModule.HolidayAggregate.Events;

public class HolidayDeletedEvent : DomainEventBase
{
  public Guid HolidayId { get; set; }

  public HolidayDeletedEvent(Guid holidayId)
  {
    HolidayId = holidayId;
  }
}

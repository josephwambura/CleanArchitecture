using Clean.Architecture.SharedKernel;

namespace Clean.Architecture.Domain.AdministrationModule.StaticSettingAggregate.Events;

public class StaticSettingDeletedEvent : DomainEventBase
{
  public Guid StaticSettingId { get; set; }

  public StaticSettingDeletedEvent(Guid staticSettingId)
  {
    StaticSettingId = staticSettingId;
  }
}

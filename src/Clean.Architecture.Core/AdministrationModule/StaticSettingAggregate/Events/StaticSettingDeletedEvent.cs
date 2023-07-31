using Clean.Architecture.SharedKernel;

namespace Clean.Architecture.Core.AdministrationModule.StaticSettingAggregate.Events;

public class StaticSettingDeletedEvent : DomainEventBase
{
  public Guid StaticSettingId { get; set; }

  public StaticSettingDeletedEvent(Guid staticSettingId)
  {
    StaticSettingId = staticSettingId;
  }
}

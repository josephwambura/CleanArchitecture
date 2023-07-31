using Clean.Architecture.SharedKernel;

namespace Clean.Architecture.Core.AdministrationModule.StaticSettingAggregate.Events;

public class StaticSettingUpdatedEvent : DomainEventBase
{
  public StaticSetting StaticSetting { get; set; }

  public StaticSettingUpdatedEvent(StaticSetting staticSetting)
  {
    StaticSetting = staticSetting;
  }
}

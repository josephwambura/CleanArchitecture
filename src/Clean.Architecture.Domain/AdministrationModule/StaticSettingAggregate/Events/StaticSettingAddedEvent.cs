using Clean.Architecture.SharedKernel;

namespace Clean.Architecture.Domain.AdministrationModule.StaticSettingAggregate.Events;

public class StaticSettingAddedEvent : DomainEventBase
{
  public StaticSetting StaticSetting { get; set; }

  public StaticSettingAddedEvent(StaticSetting staticSetting)
  {
    StaticSetting = staticSetting;
  }
}

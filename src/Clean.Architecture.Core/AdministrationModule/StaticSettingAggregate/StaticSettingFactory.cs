using Clean.Architecture.SharedKernel.Extensions;
using Clean.Architecture.SharedKernel.Utils;

namespace Clean.Architecture.Core.AdministrationModule.StaticSettingAggregate;

public static class StaticSettingFactory
{
  public static StaticSetting CreateStaticSetting(string key, string value, bool isActive, bool isSecurityCritical, byte recordStatus, ServiceHeader serviceHeader, StaticSetting? original = null)
  {
    var entity = original ?? new StaticSetting();

    entity.Key = key;
    entity.Value = value;
    entity.SecurityCritical = isSecurityCritical;

    if (isActive)
    {
      entity.Activate();
    }
    else
    {
      entity.Deactivate();
    }

    EntityFactoryExtensions.BaseEntityDetails(entity, serviceHeader, original);

    return entity;
  }
}

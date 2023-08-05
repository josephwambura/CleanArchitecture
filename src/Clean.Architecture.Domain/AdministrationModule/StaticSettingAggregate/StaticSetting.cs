using Clean.Architecture.SharedKernel;
using Clean.Architecture.SharedKernel.Interfaces;

namespace Clean.Architecture.Domain.AdministrationModule.StaticSettingAggregate;

public class StaticSetting : EntityBase, IAggregateRoot
{
  public string Key { get; set; } = default!;

  public string Value { get; set; } = default!;

  public bool IsActive { get; private set; }

  public bool SecurityCritical { get; set; }

  public void Activate()
  {
    IsActive = true;
  }

  public void Deactivate()
  {
    IsActive = false;
  }
}

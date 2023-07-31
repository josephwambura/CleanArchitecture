using Clean.Architecture.SharedKernel;
using Clean.Architecture.SharedKernel.Interfaces;

namespace Clean.Architecture.Core.AdministrationModule.StaticSettingAggregate;

public class StaticSetting : EntityBase, IAggregateRoot
{
  public string Key { get; set; } = default!;

  public string Value { get; set; } = default!;

  public bool IsActive { get; private set; }

  public bool SecurityCritical { get; set; }

  public void Activate()
  {
    this.IsActive = true;
  }

  public void Deactivate()
  {
    this.IsActive = false;
  }
}

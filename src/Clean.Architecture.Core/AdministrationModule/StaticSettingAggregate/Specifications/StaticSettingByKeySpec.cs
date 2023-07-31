using Ardalis.Specification;

namespace Clean.Architecture.Core.AdministrationModule.StaticSettingAggregate.Specifications;

public class StaticSettingByKeySpec : Specification<StaticSetting>, ISingleResultSpecification
{
  public StaticSettingByKeySpec(string key)
  {
    Query
        .Where(staticSetting => staticSetting.Key == key);
  }
}

using Ardalis.Specification;

namespace Clean.Architecture.Domain.AdministrationModule.StaticSettingAggregate.Specifications;

public class StaticSettingByKeySpec : Specification<StaticSetting>, ISingleResultSpecification<StaticSetting>
{
  public StaticSettingByKeySpec(string key)
  {
    Query
        .Where(staticSetting => staticSetting.Key == key);
  }
}

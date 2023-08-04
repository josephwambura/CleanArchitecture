using Ardalis.Specification;

using Clean.Architecture.SharedKernel.Extensions;

namespace Clean.Architecture.Core.AdministrationModule.StaticSettingAggregate.Specifications;

public class StaticSettingsWithFiltersAndInPageSpec : Specification<StaticSetting>, ISingleResultSpecification<StaticSetting>
{
  public StaticSettingsWithFiltersAndInPageSpec(string? searchString, string sortColumn, string sortDirection)
  {
    if (!string.IsNullOrWhiteSpace(searchString))
    {
      Query
        .Where(staticSetting => staticSetting.Key.Contains(searchString));
    }

    Query.ApplyOrdering(sortColumn, sortDirection);
  }
}

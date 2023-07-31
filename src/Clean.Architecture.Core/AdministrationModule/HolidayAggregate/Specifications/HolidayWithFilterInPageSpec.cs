using Ardalis.Specification;
using Clean.Architecture.SharedKernel.Extensions;

namespace Clean.Architecture.Core.AdministrationModule.HolidayAggregate.Specifications;

public class HolidaysWithFiltersAndInPageSpec : Specification<Holiday>, ISingleResultSpecification
{
  public HolidaysWithFiltersAndInPageSpec(string? searchString, string sortColumn, string sortDirection)
  {
    if (!string.IsNullOrWhiteSpace(searchString))
    {
      Query
        .Where(holiday => holiday.Description.Contains(searchString));
    }

    Query.ApplyOrdering(sortColumn, sortDirection);

    //Query
    //    .EnableCache(nameof(HolidaysWithFiltersAndInPageSpec));
  }
}

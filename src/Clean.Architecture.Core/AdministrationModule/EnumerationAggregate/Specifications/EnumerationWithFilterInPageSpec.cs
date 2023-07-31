using Ardalis.Specification;

using Clean.Architecture.SharedKernel.Extensions;

namespace Clean.Architecture.Core.AdministrationModule.EnumerationAggregate.Specifications;

public class EnumerationsWithFiltersAndInPageSpec : Specification<Enumeration>, ISingleResultSpecification
{
  public EnumerationsWithFiltersAndInPageSpec(string? searchString, string sortColumn, string sortDirection)
  {
    if (!string.IsNullOrWhiteSpace(searchString))
    {
      Query
        .Where(enumeration => enumeration.Key.Contains(searchString))
        .Where(enumeration => enumeration.Description.Contains(searchString));
    }

    Query.ApplyOrdering(sortColumn, sortDirection);

    //Query
    //    .EnableCache(nameof(EnumerationsWithFiltersAndInPageSpec));
  }
}

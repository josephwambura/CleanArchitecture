using Ardalis.Specification;

using Clean.Architecture.Domain.AdministrationModule.EnumerationAggregate;

using Clean.Architecture.SharedKernel.Extensions;

namespace Clean.Architecture.Domain.AdministrationModule.EnumerationAggregate.Specifications;

public class EnumerationsWithFiltersAndInPageSpec : Specification<Enumeration>, ISingleResultSpecification<Enumeration>
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

using Ardalis.Specification;

using Clean.Architecture.SharedKernel.Extensions;

namespace Clean.Architecture.Core.AdministrationModule.CompanyAggregate.Specifications;

public class CompaniesWithFiltersAndInPageSpec : Specification<Company>, ISingleResultSpecification<Company>
{
  public CompaniesWithFiltersAndInPageSpec(string? searchString, string sortColumn, string sortDirection)
  {
    if (!string.IsNullOrWhiteSpace(searchString))
    {
      Query
        .Where(company => company.Name.Contains(searchString));
    }

    Query.ApplyOrdering(sortColumn, sortDirection);
  }
}

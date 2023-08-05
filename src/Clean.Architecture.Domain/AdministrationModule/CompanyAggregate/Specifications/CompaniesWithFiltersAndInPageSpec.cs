using Ardalis.Specification;

using Clean.Architecture.Domain.AdministrationModule.CompanyAggregate;

using Clean.Architecture.SharedKernel.Extensions;

namespace Clean.Architecture.Domain.AdministrationModule.CompanyAggregate.Specifications;

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

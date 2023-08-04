using Ardalis.Specification;

using Clean.Architecture.SharedKernel.Extensions;

namespace Clean.Architecture.Core.InventoryModule.ProductAggregate.Specifications;

public class ProductsWithFiltersAndInPageSpec : Specification<Product>, ISingleResultSpecification<Product>
{
  public ProductsWithFiltersAndInPageSpec(string? searchString, string sortColumn, string sortDirection)
  {
    if (!string.IsNullOrWhiteSpace(searchString))
    {
      Query
        .Where(product => product.Name.Contains(searchString));
    }

    Query.ApplyOrdering(sortColumn, sortDirection);
  }
}

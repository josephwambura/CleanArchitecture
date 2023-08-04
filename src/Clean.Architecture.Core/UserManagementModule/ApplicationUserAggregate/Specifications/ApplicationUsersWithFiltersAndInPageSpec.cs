using Ardalis.Specification;

using Clean.Architecture.SharedKernel.Extensions;

namespace Clean.Architecture.Core.UserManagementModule.ApplicationUserAggregate.Specifications;

public class ApplicationUsersWithFiltersAndInPageSpec : Specification<ApplicationUser>, ISingleResultSpecification<ApplicationUser>
{
  public ApplicationUsersWithFiltersAndInPageSpec(string? searchString, string sortColumn, string sortDirection)
  {
    if (!string.IsNullOrWhiteSpace(searchString))
    {
      Query
        .Where(applicationUser => applicationUser.UserName!.Contains(searchString) || applicationUser.Email!.Contains(searchString) || applicationUser.PhoneNumber!.Contains(searchString));
    }

    Query.ApplyOrdering(sortColumn, sortDirection);
  }
}

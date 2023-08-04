using Ardalis.Specification;

using Clean.Architecture.SharedKernel.Extensions;

namespace Clean.Architecture.Core.MessagingModule.FCMAlertAggregate.Specifications;

public class FCMAlertsWithFiltersAndInPageSpec : Specification<FCMAlert>, ISingleResultSpecification<FCMAlert>
{
  public FCMAlertsWithFiltersAndInPageSpec(string? searchString, string sortColumn, string sortDirection)
  {
    if (!string.IsNullOrWhiteSpace(searchString))
    {
      Query
        .Where(fCMAlert => fCMAlert.Name.Contains(searchString));
    }

    Query.ApplyOrdering(sortColumn, sortDirection);

    //Query
    //    .EnableCache(nameof(FCMAlertsWithFiltersAndInPageSpec));
  }
}

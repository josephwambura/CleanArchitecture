using Ardalis.Specification;

using Clean.Architecture.Domain.MessagingModule.TextAlertAggregate;

using Clean.Architecture.SharedKernel.Extensions;

namespace Clean.Architecture.Domain.MessagingModule.TextAlertAggregate.Specifications;

public class TextAlertsWithFiltersAndInPageSpec : Specification<TextAlert>, ISingleResultSpecification<TextAlert>
{
  public TextAlertsWithFiltersAndInPageSpec(string? searchString, string sortColumn, string sortDirection)
  {
    if (!string.IsNullOrWhiteSpace(searchString))
    {
      Query
        .Where(textAlert => textAlert.TextMessage.Body.Contains(searchString));
    }

    Query.ApplyOrdering(sortColumn, sortDirection);
  }
}

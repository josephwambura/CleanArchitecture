using Ardalis.Specification;

using Clean.Architecture.SharedKernel.Extensions;

namespace Clean.Architecture.Core.MessagingModule.EmailAlertAggregate.Specifications;

public class EmailAlertsWithFiltersAndInPageSpec : Specification<EmailAlert>, ISingleResultSpecification
{
  public EmailAlertsWithFiltersAndInPageSpec(string? searchString, string sortColumn, string sortDirection)
  {
    if (!string.IsNullOrWhiteSpace(searchString))
    {
      Query
        .Where(emailAlert => emailAlert.EmailMessage.Body.Contains(searchString));
    }

    Query.ApplyOrdering(sortColumn, sortDirection);

    //Query
    //    .EnableCache(nameof(EmailAlertsWithFiltersAndInPageSpec));
  }
}

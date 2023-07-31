using Ardalis.Specification;
using Clean.Architecture.SharedKernel.Extensions;

namespace Clean.Architecture.Core.MessagingModule.NotificationRecipientAggregate.Specifications;

public class NotificationRecipientsWithFiltersAndInPageSpec : Specification<NotificationRecipient>, ISingleResultSpecification
{
  public NotificationRecipientsWithFiltersAndInPageSpec(string? searchString, string sortColumn, string sortDirection)
  {
    if (!string.IsNullOrWhiteSpace(searchString))
    {
      Query
        .Where(notificationRecipient => notificationRecipient.Name.Contains(searchString));
    }

    Query.ApplyOrdering(sortColumn, sortDirection);

    //Query
    //    .EnableCache(nameof(NotificationRecipientsWithFiltersAndInPageSpec));
  }
}

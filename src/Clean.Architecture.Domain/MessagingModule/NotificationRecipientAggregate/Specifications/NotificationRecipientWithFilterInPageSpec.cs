using Ardalis.Specification;

using Clean.Architecture.Domain.MessagingModule.NotificationRecipientAggregate;

using Clean.Architecture.SharedKernel.Extensions;

namespace Clean.Architecture.Domain.MessagingModule.NotificationRecipientAggregate.Specifications;

public class NotificationRecipientsWithFiltersAndInPageSpec : Specification<NotificationRecipient>, ISingleResultSpecification<NotificationRecipient>
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

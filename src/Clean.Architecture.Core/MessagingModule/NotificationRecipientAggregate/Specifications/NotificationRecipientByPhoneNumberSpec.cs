using Ardalis.Specification;

namespace Clean.Architecture.Core.MessagingModule.NotificationRecipientAggregate.Specifications;

public class NotificationRecipientByPhoneNumberSpec : Specification<NotificationRecipient>, ISingleResultSpecification<NotificationRecipient>
{
  public NotificationRecipientByPhoneNumberSpec(string phoneNumber)
  {
    Query
        .Where(notificationRecipient => notificationRecipient.Address.PhoneNumber == phoneNumber);
  }
}

using Ardalis.Specification;

using Clean.Architecture.Domain.MessagingModule.NotificationRecipientAggregate;

namespace Clean.Architecture.Domain.MessagingModule.NotificationRecipientAggregate.Specifications;

public class NotificationRecipientByPhoneNumberSpec : Specification<NotificationRecipient>, ISingleResultSpecification<NotificationRecipient>
{
  public NotificationRecipientByPhoneNumberSpec(string phoneNumber)
  {
    Query
        .Where(notificationRecipient => notificationRecipient.Address.PhoneNumber == phoneNumber);
  }
}

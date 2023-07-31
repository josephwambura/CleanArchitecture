using Ardalis.Specification;

using Clean.Architecture.Core.MessagingModule.NotificationRecipientAggregate;

namespace Clean.Architecture.Core.MessagingModule.NotificationRecipientAggregate.Specifications;

public class NotificationRecipientByPhoneNumberSpec : Specification<NotificationRecipient>, ISingleResultSpecification
{
  public NotificationRecipientByPhoneNumberSpec(string phoneNumber)
  {
    Query
        .Where(notificationRecipient => notificationRecipient.Address.PhoneNumber == phoneNumber);
  }
}

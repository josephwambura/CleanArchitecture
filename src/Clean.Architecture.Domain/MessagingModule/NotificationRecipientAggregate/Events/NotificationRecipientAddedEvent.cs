using Clean.Architecture.Domain.MessagingModule.NotificationRecipientAggregate;
using Clean.Architecture.SharedKernel;

namespace Clean.Architecture.Domain.MessagingModule.NotificationRecipientAggregate.Events;

public class NotificationRecipientAddedEvent : DomainEventBase
{
  public NotificationRecipient NotificationRecipient { get; set; }

  public NotificationRecipientAddedEvent(NotificationRecipient notificationRecipient)
  {
    NotificationRecipient = notificationRecipient;
  }
}

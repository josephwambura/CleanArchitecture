using Clean.Architecture.SharedKernel;

namespace Clean.Architecture.Core.MessagingModule.NotificationRecipientAggregate.Events;

public class NotificationRecipientAddedEvent : DomainEventBase
{
  public NotificationRecipient NotificationRecipient { get; set; }

  public NotificationRecipientAddedEvent(NotificationRecipient notificationRecipient)
  {
    NotificationRecipient = notificationRecipient;
  }
}

using Clean.Architecture.SharedKernel;

namespace Clean.Architecture.Core.MessagingModule.NotificationRecipientAggregate.Events;

public class NotificationRecipientUpdatedEvent : DomainEventBase
{
  public NotificationRecipient NotificationRecipient { get; set; }

  public NotificationRecipientUpdatedEvent(NotificationRecipient notificationRecipient)
  {
    NotificationRecipient = notificationRecipient;
  }
}

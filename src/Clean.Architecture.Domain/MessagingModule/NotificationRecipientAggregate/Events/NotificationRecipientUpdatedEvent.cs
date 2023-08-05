using Clean.Architecture.Domain.MessagingModule.NotificationRecipientAggregate;
using Clean.Architecture.SharedKernel;

namespace Clean.Architecture.Domain.MessagingModule.NotificationRecipientAggregate.Events;

public class NotificationRecipientUpdatedEvent : DomainEventBase
{
  public NotificationRecipient NotificationRecipient { get; set; }

  public NotificationRecipientUpdatedEvent(NotificationRecipient notificationRecipient)
  {
    NotificationRecipient = notificationRecipient;
  }
}

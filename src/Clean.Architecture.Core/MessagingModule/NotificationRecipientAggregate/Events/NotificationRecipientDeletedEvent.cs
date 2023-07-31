using Clean.Architecture.SharedKernel;

namespace Clean.Architecture.Core.MessagingModule.NotificationRecipientAggregate.Events;

public class NotificationRecipientDeletedEvent : DomainEventBase
{
  public Guid NotificationRecipientId { get; set; }

  public NotificationRecipientDeletedEvent(Guid notificationRecipientId)
  {
    NotificationRecipientId = notificationRecipientId;
  }
}

using Clean.Architecture.SharedKernel;

namespace Clean.Architecture.Domain.MessagingModule.NotificationRecipientAggregate.Events;

public class NotificationRecipientDeletedEvent : DomainEventBase
{
  public Guid NotificationRecipientId { get; set; }

  public NotificationRecipientDeletedEvent(Guid notificationRecipientId)
  {
    NotificationRecipientId = notificationRecipientId;
  }
}

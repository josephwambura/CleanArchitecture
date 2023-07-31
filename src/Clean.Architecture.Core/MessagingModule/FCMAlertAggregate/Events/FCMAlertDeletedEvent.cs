using Clean.Architecture.SharedKernel;

namespace Clean.Architecture.Core.MessagingModule.FCMAlertAggregate.Events;

public class FCMAlertDeletedEvent : DomainEventBase
{
  public Guid FCMAlertId { get; set; }

  public FCMAlertDeletedEvent(Guid fCMAlertId)
  {
    FCMAlertId = fCMAlertId;
  }
}

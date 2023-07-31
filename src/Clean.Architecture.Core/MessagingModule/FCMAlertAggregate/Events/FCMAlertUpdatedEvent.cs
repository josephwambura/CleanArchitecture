using Clean.Architecture.SharedKernel;

namespace Clean.Architecture.Core.MessagingModule.FCMAlertAggregate.Events;

public class FCMAlertUpdatedEvent : DomainEventBase
{
  public FCMAlert FCMAlert { get; set; }

  public FCMAlertUpdatedEvent(FCMAlert fCMAlert)
  {
    FCMAlert = fCMAlert;
  }
}

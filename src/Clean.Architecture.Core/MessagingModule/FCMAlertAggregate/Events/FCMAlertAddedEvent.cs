using Clean.Architecture.SharedKernel;

namespace Clean.Architecture.Core.MessagingModule.FCMAlertAggregate.Events;

public class FCMAlertAddedEvent : DomainEventBase
{
  public FCMAlert FCMAlert { get; set; }

  public FCMAlertAddedEvent(FCMAlert fCMAlert)
  {
    FCMAlert = fCMAlert;
  }
}

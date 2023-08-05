using Clean.Architecture.Domain.MessagingModule.FCMAlertAggregate;
using Clean.Architecture.SharedKernel;

namespace Clean.Architecture.Domain.MessagingModule.FCMAlertAggregate.Events;

public class FCMAlertUpdatedEvent : DomainEventBase
{
  public FCMAlert FCMAlert { get; set; }

  public FCMAlertUpdatedEvent(FCMAlert fCMAlert)
  {
    FCMAlert = fCMAlert;
  }
}

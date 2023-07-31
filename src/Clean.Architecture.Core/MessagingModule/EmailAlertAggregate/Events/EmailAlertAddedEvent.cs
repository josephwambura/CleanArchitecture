using Clean.Architecture.SharedKernel;

namespace Clean.Architecture.Core.MessagingModule.EmailAlertAggregate.Events;

public class EmailAlertAddedEvent : DomainEventBase
{
  public EmailAlert EmailAlert { get; set; }

  public EmailAlertAddedEvent(EmailAlert emailAlert)
  {
    EmailAlert = emailAlert;
  }
}

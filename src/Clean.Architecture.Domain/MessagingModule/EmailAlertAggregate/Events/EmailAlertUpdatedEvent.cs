using Clean.Architecture.Domain.MessagingModule.EmailAlertAggregate;
using Clean.Architecture.SharedKernel;

namespace Clean.Architecture.Domain.MessagingModule.EmailAlertAggregate.Events;

public class EmailAlertUpdatedEvent : DomainEventBase
{
  public EmailAlert EmailAlert { get; set; }

  public EmailAlertUpdatedEvent(EmailAlert emailAlert)
  {
    EmailAlert = emailAlert;
  }
}

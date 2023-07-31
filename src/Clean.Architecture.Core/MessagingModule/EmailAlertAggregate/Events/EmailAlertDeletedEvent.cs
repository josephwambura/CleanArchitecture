using Clean.Architecture.SharedKernel;

namespace Clean.Architecture.Core.MessagingModule.EmailAlertAggregate.Events;

public class EmailAlertDeletedEvent : DomainEventBase
{
  public Guid EmailAlertId { get; set; }

  public EmailAlertDeletedEvent(Guid emailAlertId)
  {
    EmailAlertId = emailAlertId;
  }
}

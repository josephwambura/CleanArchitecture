using Clean.Architecture.SharedKernel;

namespace Clean.Architecture.Domain.MessagingModule.EmailAlertAggregate.Events;

public class EmailAlertDeletedEvent : DomainEventBase
{
  public Guid EmailAlertId { get; set; }

  public EmailAlertDeletedEvent(Guid emailAlertId)
  {
    EmailAlertId = emailAlertId;
  }
}

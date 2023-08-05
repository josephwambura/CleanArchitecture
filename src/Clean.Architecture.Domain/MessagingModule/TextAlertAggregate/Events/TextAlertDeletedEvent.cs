using Clean.Architecture.SharedKernel;

namespace Clean.Architecture.Domain.MessagingModule.TextAlertAggregate.Events;

public class TextAlertDeletedEvent : DomainEventBase
{
  public Guid TextAlertId { get; set; }

  public TextAlertDeletedEvent(Guid textAlertId)
  {
    TextAlertId = textAlertId;
  }
}

using Clean.Architecture.Domain.MessagingModule.TextAlertAggregate;
using Clean.Architecture.SharedKernel;

namespace Clean.Architecture.Domain.MessagingModule.TextAlertAggregate.Events;

public class TextAlertAddedEvent : DomainEventBase
{
  public TextAlert TextAlert { get; set; }

  public TextAlertAddedEvent(TextAlert textAlert)
  {
    TextAlert = textAlert;
  }
}

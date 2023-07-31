using Clean.Architecture.SharedKernel;

namespace Clean.Architecture.Core.MessagingModule.TextAlertAggregate.Events;

public class TextAlertAddedEvent : DomainEventBase
{
  public TextAlert TextAlert { get; set; }

  public TextAlertAddedEvent(TextAlert textAlert)
  {
    TextAlert = textAlert;
  }
}

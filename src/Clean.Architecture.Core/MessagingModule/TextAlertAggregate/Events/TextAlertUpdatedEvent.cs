using Clean.Architecture.SharedKernel;

namespace Clean.Architecture.Core.MessagingModule.TextAlertAggregate.Events;

public class TextAlertUpdatedEvent : DomainEventBase
{
  public TextAlert TextAlert { get; set; }

  public TextAlertUpdatedEvent(TextAlert textAlert)
  {
    TextAlert = textAlert;
  }
}

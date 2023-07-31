using Clean.Architecture.Core.ValueObjects;
using Clean.Architecture.SharedKernel;
using Clean.Architecture.SharedKernel.Interfaces;

namespace Clean.Architecture.Core.MessagingModule.TextAlertAggregate;

public class TextAlert : EntityBase, IAggregateRoot
{
  public virtual TextMessage TextMessage { get; set; } = default!;
  public byte DlrStatus { get; set; }
  public byte Status { get; set; }
  public string? StatusDescription { get; set; }
  public string? Reference { get; set; }
  public byte Origin { get; set; }
  public byte Priority { get; set; }
  public byte SendRetry { get; set; }
}

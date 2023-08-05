using Clean.Architecture.Domain.ValueObjects;
using Clean.Architecture.SharedKernel;
using Clean.Architecture.SharedKernel.Interfaces;

namespace Clean.Architecture.Domain.MessagingModule.EmailAlertAggregate;

public class EmailAlert : EntityBase, IAggregateRoot
{
  public string From { get; set; } = default!;
  public virtual EmailMessage EmailMessage { get; set; } = default!;
  public byte DLRStatus { get; set; }
  public string? Reference { get; set; }
  public byte Origin { get; set; }
  public string? Catalyst { get; set; }
  public byte Priority { get; set; }
  public int SendRetry { get; set; }
}

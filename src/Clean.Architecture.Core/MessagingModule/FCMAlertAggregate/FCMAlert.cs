using Ardalis.GuardClauses;

using Clean.Architecture.SharedKernel;
using Clean.Architecture.SharedKernel.Interfaces;
using Clean.Architecture.SharedKernel.Utils;

namespace Clean.Architecture.Core.MessagingModule.FCMAlertAggregate;

public class FCMAlert : EntityBase, IAggregateRoot
{
  public string Name { get; set; } = default!;

  public PriorityStatus Priority { get; set; } = default!;

  public void UpdateName(string newName)
  {
    Name = Guard.Against.NullOrEmpty(newName, nameof(newName));
  }
}

using Ardalis.GuardClauses;

using Clean.Architecture.Domain.ValueObjects;
using Clean.Architecture.SharedKernel;
using Clean.Architecture.SharedKernel.Interfaces;

namespace Clean.Architecture.Domain.MessagingModule.NotificationRecipientAggregate;

public class NotificationRecipient : EntityBase, IAggregateRoot
{
  public string Name { get; set; } = default!;
  public virtual Address Address { get; set; } = default!;
  public bool IsEnabled { get; set; }

  public void Disable()
  {
    IsEnabled = false;
  }

  public void Enable()
  {
    IsEnabled = true;
  }

  public void UpdateName(string newName)
  {
    Name = Guard.Against.NullOrEmpty(newName, nameof(newName));
  }
}

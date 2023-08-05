using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

using Clean.Architecture.Domain.ValueObjects;
using Clean.Architecture.SharedKernel;
using Clean.Architecture.SharedKernel.Interfaces;

using Microsoft.AspNetCore.Identity;

namespace Clean.Architecture.Domain.UserManagementModule.ApplicationUserAggregate;

public class ApplicationUser : IdentityUser, IAggregateRoot
{
  [MaxLength(100)]
  public string? FirstName { get; set; }

  [MaxLength(100)]
  public string? MiddleName { get; set; }

  [MaxLength(100)]
  public string? LastName { get; set; }

  [MaxLength(256)]
  public string? ProfilePicture { get; set; }

  public bool IsEnabled { get; set; }

  public DateTime? LastLoggedInDate { get; set; }

  public DateTime? LastPasswordChangedDate { get; set; }

  public virtual TransactionEnvironment TransactionEnvironment { get; set; } = default!;

  public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

  public DateTime? ModifiedDate { get; set; }

  public string CreatedBy { get; set; } = default!;

  public string? ModifiedBy { get; set; }

  public byte RecordStatus { get; set; }

  private List<DomainEventBase> _domainEvents = new();
  [NotMapped]
  public IEnumerable<DomainEventBase> DomainEvents => _domainEvents.AsReadOnly();

  public virtual IdentityUser ShallowClone()
  {
    return (IdentityUser)MemberwiseClone();
  }

  public virtual IdentityUser? DeepClone()
  {
    var json = JsonSerializer.Serialize(this);
    return JsonSerializer.Deserialize<IdentityUser>(json);
  }

  /// <summary>
  ///   Check if this entity is transient, ie, without identity at this moment
  /// </summary>
  /// <returns> True if entity is transient, else false </returns>
  public bool IsTransient()
  {
    return string.IsNullOrWhiteSpace(Id);
  }

  protected void RegisterDomainEvent(DomainEventBase domainEvent) => _domainEvents.Add(domainEvent);
  public void ClearDomainEvents() => _domainEvents.Clear();
}

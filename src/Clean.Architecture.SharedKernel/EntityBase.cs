using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace Clean.Architecture.SharedKernel;

// This can be modified to EntityBase<TId> to support multiple key types (e.g. Guid)
public abstract class EntityBase
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public virtual Guid Id { get; set; }

  /// <summary>
  ///   Check if this entity is transient, ie, without identity at this moment
  /// </summary>
  /// <returns> True if entity is transient, else false </returns>
  public bool IsTransient()
  {
    return Id == Guid.Empty;
  }

  public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;

  public DateTimeOffset? ModifiedDate { get; set; }

  public string CreatedBy { get; set; } = default!;

  public string? ModifiedBy { get; set; }

  public byte RecordStatus { get; set; }

  private List<DomainEventBase> _domainEvents = new();
  [NotMapped]
  public IEnumerable<DomainEventBase> DomainEvents => _domainEvents.AsReadOnly();

  public virtual EntityBase ShallowClone()
  {
    return (EntityBase)this.MemberwiseClone();
  }

  public virtual EntityBase? DeepClone()
  {
    string json = JsonSerializer.Serialize(this);
    return JsonSerializer.Deserialize<EntityBase>(json);
  }

  protected void RegisterDomainEvent(DomainEventBase domainEvent) => _domainEvents.Add(domainEvent);
  internal void ClearDomainEvents() => _domainEvents.Clear();
}

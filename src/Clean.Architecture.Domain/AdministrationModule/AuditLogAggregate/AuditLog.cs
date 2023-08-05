using Clean.Architecture.Domain.ValueObjects;
using Clean.Architecture.SharedKernel;
using Clean.Architecture.SharedKernel.Interfaces;

namespace Clean.Architecture.Domain.AdministrationModule.AuditLogAggregate;

public class AuditLog : EntityBase, IAggregateRoot
{
  public string EventType { get; set; } = default!;
  public string TableName { get; set; } = default!;
  public string RecordID { get; set; } = default!;
  public string AdditionalNarration { get; set; } = default!;
  public string ApplicationDomainName { get; set; } = default!;
  public string ApplicationUserName { get; set; } = default!;
  public TransactionEnvironment TransactionEnvironment { get; set; } = default!;
}

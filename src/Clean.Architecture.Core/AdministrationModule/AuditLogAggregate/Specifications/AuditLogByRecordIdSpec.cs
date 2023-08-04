using Ardalis.Specification;

namespace Clean.Architecture.Core.AdministrationModule.AuditLogAggregate.Specifications;

public class AuditLogByRecordIdSpec : Specification<AuditLog>, ISingleResultSpecification<AuditLog>
{
  public AuditLogByRecordIdSpec(string phoneNumber)
  {
    Query
        .Where(auditLog => auditLog.RecordID == phoneNumber);
  }
}

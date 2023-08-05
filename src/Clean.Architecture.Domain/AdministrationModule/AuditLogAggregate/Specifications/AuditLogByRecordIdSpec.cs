using Ardalis.Specification;

using Clean.Architecture.Domain.AdministrationModule.AuditLogAggregate;

namespace Clean.Architecture.Domain.AdministrationModule.AuditLogAggregate.Specifications;

public class AuditLogByRecordIdSpec : Specification<AuditLog>, ISingleResultSpecification<AuditLog>
{
  public AuditLogByRecordIdSpec(string phoneNumber)
  {
    Query
        .Where(auditLog => auditLog.RecordID == phoneNumber);
  }
}

using Ardalis.Specification;

using Clean.Architecture.SharedKernel.Extensions;

namespace Clean.Architecture.Core.AdministrationModule.AuditLogAggregate.Specifications;

public class AuditLogsWithFiltersAndInPageSpec : Specification<AuditLog>, ISingleResultSpecification
{
  public AuditLogsWithFiltersAndInPageSpec(string? searchString, string sortColumn, string sortDirection)
  {
    if (!string.IsNullOrWhiteSpace(searchString))
    {
      Query
        .Where(auditLog => auditLog.AdditionalNarration.Contains(searchString));
    }

    Query.ApplyOrdering(sortColumn, sortDirection);

    //Query
    //    .EnableCache(nameof(AuditLogsWithFiltersAndInPageSpec));
  }
}

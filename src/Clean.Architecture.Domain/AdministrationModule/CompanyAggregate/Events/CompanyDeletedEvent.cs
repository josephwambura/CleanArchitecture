using Clean.Architecture.SharedKernel;

namespace Clean.Architecture.Domain.AdministrationModule.CompanyAggregate.Events;

public class CompanyDeletedEvent : DomainEventBase
{
  public Guid CompanyId { get; set; }

  public CompanyDeletedEvent(Guid companyId)
  {
    CompanyId = companyId;
  }
}

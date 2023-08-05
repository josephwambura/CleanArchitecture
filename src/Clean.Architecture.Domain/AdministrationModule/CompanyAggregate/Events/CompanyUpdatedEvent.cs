using Clean.Architecture.Domain.AdministrationModule.CompanyAggregate;
using Clean.Architecture.SharedKernel;

namespace Clean.Architecture.Domain.AdministrationModule.CompanyAggregate.Events;

public class CompanyUpdatedEvent : DomainEventBase
{
  public Company Company { get; set; }

  public CompanyUpdatedEvent(Company company)
  {
    Company = company;
  }
}

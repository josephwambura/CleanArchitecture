using Clean.Architecture.SharedKernel;

namespace Clean.Architecture.Domain.AdministrationModule.CompanyAggregate.Events;

public class CompanyAddedEvent : DomainEventBase
{
  public Company Company { get; set; }

  public CompanyAddedEvent(Company company)
  {
    Company = company;
  }
}

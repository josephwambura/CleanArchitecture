using Clean.Architecture.SharedKernel;

namespace Clean.Architecture.Core.AdministrationModule.CompanyAggregate.Events;

public class CompanyUpdatedEvent : DomainEventBase
{
  public Company Company { get; set; }

  public CompanyUpdatedEvent(Company company)
  {
    Company = company;
  }
}

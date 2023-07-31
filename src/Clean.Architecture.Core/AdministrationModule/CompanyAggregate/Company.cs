using Clean.Architecture.SharedKernel;
using Clean.Architecture.SharedKernel.Interfaces;

namespace Clean.Architecture.Core.AdministrationModule.CompanyAggregate;

public class Company : EntityBase, IAggregateRoot
{
  public string Name { get; set; } = default!;
}

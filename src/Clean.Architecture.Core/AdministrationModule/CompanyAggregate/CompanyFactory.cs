using Clean.Architecture.SharedKernel.Extensions;
using Clean.Architecture.SharedKernel.Utils;

namespace Clean.Architecture.Core.AdministrationModule.CompanyAggregate;

public static class CompanyFactory
{
  public static Company CreateCompany(string name, byte recordStatus, ServiceHeader serviceHeader, Company? original = null)
  {
    var entity = original ?? new Company();

    entity.Name = name;

    EntityFactoryExtensions.BaseEntityDetails(entity, serviceHeader, original);

    return entity;
  }
}

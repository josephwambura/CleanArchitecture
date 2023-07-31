using Ardalis.GuardClauses;

using Clean.Architecture.SharedKernel.Extensions;
using Clean.Architecture.SharedKernel.Utils;

namespace Clean.Architecture.Core.AdministrationModule.EnumerationAggregate;

public static class EnumerationFactory
{
  public static Enumeration CreateEnumeration(string key, int value, string description, byte recordStatus, ServiceHeader serviceHeader, Enumeration? original = null)
  {
    Guard.Against.Null(serviceHeader, nameof(serviceHeader));

    var entity = original ?? new Enumeration();

    entity.Key = Guard.Against.NullOrEmpty(key, nameof(key));
    entity.Value = value;

    EntityFactoryExtensions.BaseEntityDetails(entity, serviceHeader, original);

    return entity;
  }
}

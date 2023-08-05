using Ardalis.GuardClauses;

using Clean.Architecture.SharedKernel.Extensions;
using Clean.Architecture.SharedKernel.Utils;

namespace Clean.Architecture.Domain.MessagingModule.FCMAlertAggregate;

public static class FCMAlertFactory
{
  public static FCMAlert CreateFCMAlert(string name, PriorityStatus priority, byte recordStatus, ServiceHeader serviceHeader, FCMAlert? original = null)
  {
    Guard.Against.Null(serviceHeader, nameof(serviceHeader));

    var entity = original ?? new FCMAlert();

    entity.Name = Guard.Against.NullOrEmpty(name, nameof(name));
    entity.Priority = priority;

    EntityFactoryExtensions.BaseEntityDetails(entity, serviceHeader, original);

    return entity;
  }
}

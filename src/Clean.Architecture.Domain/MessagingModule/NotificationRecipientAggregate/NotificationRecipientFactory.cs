using Ardalis.GuardClauses;

using Clean.Architecture.Domain.ValueObjects;
using Clean.Architecture.SharedKernel.Extensions;
using Clean.Architecture.SharedKernel.Utils;

namespace Clean.Architecture.Domain.MessagingModule.NotificationRecipientAggregate;

public static class NotificationRecipientFactory
{
  public static NotificationRecipient CreateNotificationRecipient(string name, Address address, bool isEnabled, byte recordStatus,
      ServiceHeader serviceHeader, NotificationRecipient? original = null)
  {
    Guard.Against.Null(serviceHeader, nameof(serviceHeader));

    var entity = original ?? new NotificationRecipient();

    entity.Name = Guard.Against.NullOrEmpty(name, nameof(name));
    entity.Address = address;
    entity.IsEnabled = isEnabled;

    EntityFactoryExtensions.BaseEntityDetails(entity, serviceHeader, original);

    return entity;
  }
}

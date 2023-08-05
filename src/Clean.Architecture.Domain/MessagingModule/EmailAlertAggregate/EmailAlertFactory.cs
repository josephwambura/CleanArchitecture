using Ardalis.GuardClauses;

using Clean.Architecture.Domain.ValueObjects;
using Clean.Architecture.SharedKernel.Extensions;
using Clean.Architecture.SharedKernel.Utils;

namespace Clean.Architecture.Domain.MessagingModule.EmailAlertAggregate;

public static class EmailAlertFactory
{
  public static EmailAlert CreateEmailAlert(string from, EmailMessage emailMessage, byte dLRStatus, string? reference, byte origin, string? catalyst, byte priority, int sendRetry, byte recordStatus, ServiceHeader serviceHeader, EmailAlert? original = null)
  {
    Guard.Against.Null(serviceHeader, nameof(serviceHeader));

    var entity = original ?? new EmailAlert();

    entity.From = Guard.Against.NullOrWhiteSpace(from, nameof(from));
    entity.EmailMessage = Guard.Against.Null(emailMessage, nameof(emailMessage));
    entity.DLRStatus = dLRStatus;
    entity.Reference = reference;
    entity.Origin = origin;
    entity.Catalyst = catalyst;
    entity.Priority = priority;
    entity.SendRetry = sendRetry;

    EntityFactoryExtensions.BaseEntityDetails(entity, serviceHeader, original);

    return entity;
  }
}

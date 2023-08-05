using Ardalis.GuardClauses;

using Clean.Architecture.Domain.ValueObjects;
using Clean.Architecture.SharedKernel.Extensions;
using Clean.Architecture.SharedKernel.Utils;

namespace Clean.Architecture.Domain.MessagingModule.TextAlertAggregate;

public static class TextAlertFactory
{
  public static TextAlert CreateTextAlert(TextMessage textMessage, byte dlrStatus, byte status, string? statusDescription, string? reference, byte origin, byte priority, byte sendRetry, byte recordStatus, ServiceHeader serviceHeader, TextAlert? original = null)
  {
    Guard.Against.Null(serviceHeader, nameof(serviceHeader));

    var entity = original ?? new TextAlert();

    entity.TextMessage = Guard.Against.Null(textMessage, nameof(textMessage));
    entity.DlrStatus = dlrStatus;
    entity.Status = status;
    entity.StatusDescription = statusDescription;
    entity.Reference = reference;
    entity.Origin = origin;
    entity.Priority = priority;
    entity.SendRetry = sendRetry;

    EntityFactoryExtensions.BaseEntityDetails(entity, serviceHeader, original);

    return entity;
  }
}

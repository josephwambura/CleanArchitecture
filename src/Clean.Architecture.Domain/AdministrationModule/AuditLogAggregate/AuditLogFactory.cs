using Ardalis.GuardClauses;

using Clean.Architecture.Domain.ValueObjects;
using Clean.Architecture.SharedKernel.Extensions;
using Clean.Architecture.SharedKernel.Utils;

namespace Clean.Architecture.Domain.AdministrationModule.AuditLogAggregate;

public static class AuditLogFactory
{
  public static AuditLog CreateAuditLog(string eventType, string tableName, string recordID,
      string additionalNarration, string applicationDomainName, string applicationUserName,
      TransactionEnvironment transactionEnvironment, byte recordStatus,
      ServiceHeader serviceHeader, AuditLog? original = null)
  {
    Guard.Against.Null(serviceHeader, nameof(serviceHeader));

    var entity = original ?? new AuditLog();

    entity.EventType = Guard.Against.NullOrEmpty(eventType, nameof(eventType));
    entity.TableName = tableName;
    entity.RecordID = recordID;
    entity.AdditionalNarration = additionalNarration;
    entity.ApplicationDomainName = applicationDomainName;
    entity.ApplicationUserName = applicationUserName;
    entity.TransactionEnvironment = transactionEnvironment;

    EntityFactoryExtensions.BaseEntityDetails(entity, serviceHeader, original);

    return entity;
  }
}

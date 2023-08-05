using Ardalis.GuardClauses;

using Clean.Architecture.SharedKernel.Extensions;
using Clean.Architecture.SharedKernel.Utils;

namespace Clean.Architecture.Domain.AdministrationModule.HolidayAggregate;

public static class HolidayFactory
{
  public static Holiday CreateHoliday(DateTime day, string description, bool isInternational, byte recordStatus,
      ServiceHeader serviceHeader, Holiday? original = null)
  {
    Guard.Against.Null(serviceHeader, nameof(serviceHeader));

    var entity = original ?? new Holiday();

    entity.Day = Guard.Against.Null(day, nameof(day));
    entity.Description = description;
    entity.IsInternational = isInternational;

    EntityFactoryExtensions.BaseEntityDetails(entity, serviceHeader, original);

    return entity;
  }
}

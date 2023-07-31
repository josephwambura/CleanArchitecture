using Ardalis.Result;
using Clean.Architecture.Application.DTO.AdministrationModule.StaticSettingAggregate;
using Clean.Architecture.SharedKernel;
using Clean.Architecture.SharedKernel.Utils;

namespace Clean.Architecture.Application.Interfaces.AdministrationModule;

public interface IStaticSettingAppService
{
  Task<Result<StaticSettingDTO>> AddStaticSettingAsync(StaticSettingDTO staticSettingDTO, ServiceHeader serviceHeader, CancellationToken cancellationToken = default);

  Task<Result> UpdateStaticSettingAsync(Guid staticSettingId, StaticSettingDTO staticSettingDTO, ServiceHeader serviceHeader, CancellationToken cancellationToken = default);

  Task<Result> DeleteStaticSettingAsync(Guid staticSettingId, ServiceHeader serviceHeader, CancellationToken cancellationToken = default);

  Task<Result<StaticSettingDTO>> FindStaticSettingAsync(Guid staticSettingId, ServiceHeader serviceHeader, CancellationToken cancellationToken = default);
  
  Task<Result<StaticSettingDTO>> FindStaticSettingByKeyAsync(string key, ServiceHeader serviceHeader, CancellationToken cancellationToken = default);
  
  Task<Result<List<StaticSettingDTO>>> FindStaticSettingsAsync(ServiceHeader serviceHeader, CancellationToken cancellationToken = default);

  Task<Result<PageCollectionInfo<StaticSettingDTO>>> GetStaticSettingsWithFiltersAndInPageAsync(string? searchString, int pageNumber, int pageSize, string sortColumn, string sortDirection, ServiceHeader serviceHeader, CancellationToken cancellationToken = default);
}

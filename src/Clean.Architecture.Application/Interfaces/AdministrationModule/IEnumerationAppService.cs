using Ardalis.Result;

using Clean.Architecture.Application.DTO.AdministrationModule.EnumerationAggregate;
using Clean.Architecture.SharedKernel;
using Clean.Architecture.SharedKernel.Utils;

namespace Clean.Architecture.Application.Core.Interfaces.AdministrationModule;

public interface IEnumerationAppService
{
  Task<Result<EnumerationDTO>> AddEnumerationAsync(EnumerationDTO enumerationDTO, ServiceHeader serviceHeader, CancellationToken cancellationToken = default);

  Task<Result<bool>> BulkInsertEnumerationsAsync(List<EnumerationDTO> enumerationDTOs, ServiceHeader serviceHeader, CancellationToken cancellationToken = default);

  Task<Result> UpdateEnumerationAsync(Guid enumerationId, EnumerationDTO enumerationDTO, ServiceHeader serviceHeader, CancellationToken cancellationToken = default);

  Task<Result> DeleteEnumerationAsync(Guid enumerationId, ServiceHeader serviceHeader, CancellationToken cancellationToken = default);

  Task<Result<EnumerationDTO>> FindEnumerationAsync(Guid enumerationId, ServiceHeader serviceHeader, CancellationToken cancellationToken = default);

  Task<Result<List<EnumerationDTO>>> FindEnumerationsAsync(ServiceHeader serviceHeader, CancellationToken cancellationToken = default);

  Task<Result<PageCollectionInfo<EnumerationDTO>>> GetEnumerationsWithFiltersAndInPageAsync(string? searchString, int pageNumber, int pageSize, string sortColumn, string sortDirection, ServiceHeader serviceHeader, CancellationToken cancellationToken = default);
}

using Ardalis.Result;
using Clean.Architecture.Application.DTO.MessagingModule.TextAlertAggregate;
using Clean.Architecture.SharedKernel;
using Clean.Architecture.SharedKernel.Utils;

namespace Clean.Architecture.Application.Core.Interfaces.MessagingModule;

public interface ITextAlertAppService
{
  Task<Result<TextAlertDTO>> AddTextAlertAsync(TextAlertDTO textAlertDTO, ServiceHeader serviceHeader, CancellationToken cancellationToken = default);

  Task<Result<bool>> BulkInsertTextAlertsAsync(List<TextAlertDTO> textAlertDTOs, ServiceHeader serviceHeader, CancellationToken cancellationToken = default);

  Task<Result> UpdateTextAlertAsync(Guid textAlertId, TextAlertDTO textAlertDTO, ServiceHeader serviceHeader, CancellationToken cancellationToken = default);

  Task<Result> DeleteTextAlertAsync(Guid textAlertId, ServiceHeader serviceHeader, CancellationToken cancellationToken = default);

  Task<Result<TextAlertDTO>> FindTextAlertAsync(Guid textAlertId, ServiceHeader serviceHeader, CancellationToken cancellationToken = default);

  Task<Result<List<TextAlertDTO>>> FindTextAlertsAsync(ServiceHeader serviceHeader, CancellationToken cancellationToken = default);

  Task<Result<PageCollectionInfo<TextAlertDTO>>> GetTextAlertsWithFiltersAndInPageAsync(string? searchString, int pageNumber, int pageSize, string sortColumn, string sortDirection, ServiceHeader serviceHeader, CancellationToken cancellationToken = default);
}

using Ardalis.Result;
using Clean.Architecture.Application.DTO.MessagingModule.EmailAlertAggregate;
using Clean.Architecture.SharedKernel;
using Clean.Architecture.SharedKernel.Utils;

namespace Clean.Architecture.Application.Core.Interfaces.MessagingModule;

public interface IEmailAlertAppService
{
  Task<Result<EmailAlertDTO>> AddEmailAlertAsync(EmailAlertDTO emailAlertDTO, ServiceHeader serviceHeader, CancellationToken cancellationToken = default);

  Task<Result<bool>> BulkInsertEmailAlertsAsync(List<EmailAlertDTO> emailAlertDTOs, ServiceHeader serviceHeader, CancellationToken cancellationToken = default);

  Task<Result> UpdateEmailAlertAsync(Guid emailAlertId, EmailAlertDTO emailAlertDTO, ServiceHeader serviceHeader, CancellationToken cancellationToken = default);
  
  Task<Result> MarkQueuedEmailAlertAsync(Guid emailAlertId, ServiceHeader serviceHeader, CancellationToken cancellationToken = default);
  
  Task<Result> MarkDeliveredEmailAlertAsync(Guid emailAlertId, ServiceHeader serviceHeader, CancellationToken cancellationToken = default);

  Task<Result> DeleteEmailAlertAsync(Guid emailAlertId, ServiceHeader serviceHeader, CancellationToken cancellationToken = default);

  Task<Result<EmailAlertDTO>> FindEmailAlertAsync(Guid emailAlertId, ServiceHeader serviceHeader, CancellationToken cancellationToken = default);

  Task<Result<List<EmailAlertDTO>>> FindEmailAlertsAsync(ServiceHeader serviceHeader, CancellationToken cancellationToken = default);

  Task<Result<PageCollectionInfo<EmailAlertDTO>>> GetEmailAlertsWithFiltersAndInPageAsync(string? searchString, int pageNumber, int pageSize, string sortColumn, string sortDirection, ServiceHeader serviceHeader, CancellationToken cancellationToken = default);

  Task<Result<List<EmailAlertDTO>>> GetEmailAlertsByDLRStatusAndWithFiltersAndInPageAsync(byte[] dlrStatuses, string? searchString, int pageSize, string sortColumn, string sortDirection, ServiceHeader serviceHeader, CancellationToken cancellationToken = default);
}

using Ardalis.Result;

using Clean.Architecture.Application.DTO;
using Clean.Architecture.SharedKernel.Utils;

namespace Clean.Architecture.Application.Interfaces;

public interface IBrokerServiceAppService
{
  Task<Result<DashboardDTO>> FindDashboardsAsync(ServiceHeader serviceHeader, CancellationToken cancellationToken = default);
}

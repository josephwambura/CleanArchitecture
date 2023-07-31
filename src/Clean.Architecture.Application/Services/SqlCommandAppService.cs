using Ardalis.Result;

using Clean.Architecture.Application.DTO;
using Clean.Architecture.Application.Interfaces;
using Clean.Architecture.Core;
using Clean.Architecture.SharedKernel.Interfaces;
using Clean.Architecture.SharedKernel.Utils;

using MediatR;

namespace Clean.Architecture.Application.Services;

public class SqlCommandAppService : ISqlCommandAppService
{
  private readonly IMediator _mediator;
  private readonly IUnitOfWork _unitOfWork;

  public SqlCommandAppService(IUnitOfWork unitOfWork, IMediator mediator)
  {
    _unitOfWork = unitOfWork;
    _mediator = mediator;
  }

  public async Task<Result<DashboardDTO>> FindDashboardsAsync(ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      var productDTOs = await (_unitOfWork.GetIReadRepository<Dashboard>((byte)DbConnectionContext.LIVE))!.ListAsync<DashboardDTO>(cts.Token);

      if (productDTOs == null || productDTOs.FirstOrDefault() == null)
      {
        return Result.NotFound();
      }

      return new Result<DashboardDTO>(productDTOs.FirstOrDefault()!);
    }
    catch (Exception ex)
    {
      // TODO: Log details here
      return Result.Error(new[] { ex.Message });
    }
  }

}

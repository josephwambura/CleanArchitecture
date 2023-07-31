using Ardalis.Result;

using Clean.Architecture.Application.DTO.AdministrationModule.CompanyAggregate;
using Clean.Architecture.Application.Interfaces.AdministrationModule;
using Clean.Architecture.Core.AdministrationModule.CompanyAggregate;
using Clean.Architecture.Core.AdministrationModule.CompanyAggregate.Events;
using Clean.Architecture.Core.AdministrationModule.CompanyAggregate.Specifications;
using Clean.Architecture.SharedKernel;
using Clean.Architecture.SharedKernel.Interfaces;
using Clean.Architecture.SharedKernel.Utils;

using MediatR;

namespace Clean.Architecture.Application.Services.AdministrationModule;

public class CompanyAppService : ICompanyAppService
{
  private readonly IUnitOfWork _unitOfWork;
  private readonly IReadRepository<Company>? _readRepository;
  private readonly IRepository<Company>? _repository;
  private readonly IMediator _mediator;

  public CompanyAppService(IUnitOfWork unitOfWork, IMediator mediator)
  {
    _unitOfWork = unitOfWork;
    _mediator = mediator;

    _readRepository = _unitOfWork.GetIReadRepository<Company>((byte)DbConnectionContext.LIVE);
    _repository = _unitOfWork.GetIRepository<Company>((byte)DbConnectionContext.LIVE);
  }

  public async Task<Result<CompanyDTO>> AddCompanyAsync(CompanyDTO companyDTO, ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      var aggregateToAdd = CompanyFactory.CreateCompany(companyDTO.Name, (byte)RecordStatus.Approved, serviceHeader);

      var result = await _repository!.AddAsync(aggregateToAdd, cts.Token);
      await _unitOfWork.SaveAsync();
      var domainEvent = new CompanyAddedEvent(result);
      await _mediator.Publish(domainEvent, cts.Token).ConfigureAwait(false);
      return Result.Success(_unitOfWork.MapTo<CompanyDTO>(result));
    }
    catch (Exception ex)
    {
      // TODO: Log details here
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result> UpdateCompanyAsync(Guid companyId, CompanyDTO companyDTO, ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      var aggregateToUpdate = await _repository!.GetByIdAsync(companyId, cts.Token);
      if (aggregateToUpdate == null) return Result.NotFound();

      aggregateToUpdate = CompanyFactory.CreateCompany(companyDTO.Name, (byte)RecordStatus.Approved, serviceHeader, aggregateToUpdate);

      await _repository!.UpdateAsync(aggregateToUpdate, cts.Token);
      await _unitOfWork.SaveAsync();
      var domainEvent = new CompanyUpdatedEvent(aggregateToUpdate);
      await _mediator.Publish(domainEvent, cts.Token).ConfigureAwait(false);
      return Result.Success();
    }
    catch (Exception ex)
    {
      // TODO: Log details here
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result> DeleteCompanyAsync(Guid companyId, ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      var aggregateToDelete = await _readRepository!.GetByIdAsync(companyId, cts.Token);
      if (aggregateToDelete == null) return Result.NotFound();

      await _repository!.DeleteAsync(aggregateToDelete, cts.Token);
      var domainEvent = new CompanyDeletedEvent(companyId);
      await _mediator.Publish(domainEvent, cts.Token).ConfigureAwait(false);
      return Result.Success();
    }
    catch (Exception ex)
    {
      // TODO: Log details here
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result<CompanyDTO>> FindCompanyAsync(Guid companyId, ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      var aggregate = await _readRepository!.GetByIdAsync(companyId, cts.Token);

      if (aggregate == null)
      {
        return Result.NotFound();
      }

      return new Result<CompanyDTO>(_unitOfWork.MapTo<CompanyDTO>(aggregate));
    }
    catch (Exception ex)
    {
      // TODO: Log details here
      return Result.Error(new[] { ex.Message });
    }
  }
  
  public async Task<Result<List<CompanyDTO>>> FindCompaniesAsync(ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      var companyDTOs = await _readRepository!.ListAsync<CompanyDTO>(cts.Token);

      if (companyDTOs == null)
      {
        return Result.NotFound();
      }

      return new Result<List<CompanyDTO>>(companyDTOs);
    }
    catch (Exception ex)
    {
      // TODO: Log details here
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result<PageCollectionInfo<CompanyDTO>>> GetCompaniesWithFiltersAndInPageAsync(string? searchString, int pageNumber, int pageSize, string sortColumn, string sortDirection, ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      var allCompanySpec = new CompaniesWithFiltersAndInPageSpec(searchString, sortColumn, sortDirection);

      var companies = await PaginatedIAggregateRootList<Company, CompanyDTO>.CreatePageCollectionInfoAsync(_unitOfWork, allCompanySpec, pageNumber, pageSize, _unitOfWork.MapTo<CompanyDTO>, cts.Token);

      if (companies != null)
      {
        if (companies.PageCollection == null)
        {
          return Result<PageCollectionInfo<CompanyDTO>>.NotFound();
        }

        return new Result<PageCollectionInfo<CompanyDTO>>(companies);
      }

      return Result<PageCollectionInfo<CompanyDTO>>.NotFound("");
    }
    catch (Exception ex)
    {
      // TODO: Log details here
      return Result<PageCollectionInfo<CompanyDTO>>.Error(new[] { ex.Message });
    }
  }
}

using Ardalis.Result;

using Clean.Architecture.Application.DTO.AdministrationModule.CompanyAggregate;
using Clean.Architecture.SharedKernel;
using Clean.Architecture.SharedKernel.Utils;

namespace Clean.Architecture.Application.Interfaces.AdministrationModule;

public interface ICompanyAppService
{
  Task<Result<CompanyDTO>> AddCompanyAsync(CompanyDTO companyDTO, ServiceHeader serviceHeader, CancellationToken cancellationToken = default);

  Task<Result> UpdateCompanyAsync(Guid companyId, CompanyDTO companyDTO, ServiceHeader serviceHeader, CancellationToken cancellationToken = default);

  Task<Result> DeleteCompanyAsync(Guid companyId, ServiceHeader serviceHeader, CancellationToken cancellationToken = default);

  Task<Result<CompanyDTO>> FindCompanyAsync(Guid companyId, ServiceHeader serviceHeader, CancellationToken cancellationToken = default);

  Task<Result<List<CompanyDTO>>> FindCompaniesAsync(ServiceHeader serviceHeader, CancellationToken cancellationToken = default);

  Task<Result<PageCollectionInfo<CompanyDTO>>> GetCompaniesWithFiltersAndInPageAsync(string? searchString, int pageNumber, int pageSize, string sortColumn, string sortDirection, ServiceHeader serviceHeader, CancellationToken cancellationToken = default);
}

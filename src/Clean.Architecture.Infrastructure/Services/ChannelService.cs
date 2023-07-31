using Clean.Architecture.Application.Core.Interfaces.AdministrationModule;
using Clean.Architecture.Application.Core.Interfaces.MessagingModule;
using Clean.Architecture.Application.DTO;
using Clean.Architecture.Application.DTO.AdministrationModule.CompanyAggregate;
using Clean.Architecture.Application.DTO.AdministrationModule.EnumerationAggregate;
using Clean.Architecture.Application.DTO.AdministrationModule.StaticSettingAggregate;
using Clean.Architecture.Application.DTO.InventoryModule.ProductAggregate;
using Clean.Architecture.Application.DTO.MessagingModule.EmailAlertAggregate;
using Clean.Architecture.Application.DTO.MessagingModule.TextAlertAggregate;
using Clean.Architecture.Application.DTO.ProjectAggregate;
using Clean.Architecture.Application.DTO.UserManagementModule.ApplicationUserAggregate;
using Clean.Architecture.Application.Interfaces;
using Clean.Architecture.Application.Interfaces.AdministrationModule;
using Clean.Architecture.Application.Interfaces.InventoryModule;
using Clean.Architecture.Application.Interfaces.UserManagementModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Clean.Architecture.Infrastructure.Services;

public class ChannelService : IChannelService
{
  private readonly Lazy<IProjectService> _projectService;
  private readonly Lazy<ICompanyAppService> _companyService;
  private readonly Lazy<IEnumerationAppService> _enumerationService;
  private readonly Lazy<IStaticSettingAppService> _staticSettingService;
  private readonly Lazy<IProductAppService> _productService;
  private readonly Lazy<IEmailAlertAppService> _emailAlertService;
  private readonly Lazy<ITextAlertAppService> _textAlertService;
  private readonly Lazy<IApplicationUserService> _applicationUserService;
  private readonly Lazy<ISqlCommandAppService> _sqlCommandAppService;
  private readonly Lazy<ILogger<ChannelService>> _logger;

  public ChannelService(
    Lazy<IProjectService> projectService,
    Lazy<ICompanyAppService> companyService,
    Lazy<IEnumerationAppService> enumerationService,
    Lazy<IStaticSettingAppService> staticSettingService,
    Lazy<IProductAppService> productService,
    Lazy<IEmailAlertAppService> emailAlertService,
    Lazy<ITextAlertAppService> textAlertService,
    Lazy<IApplicationUserService> applicationUserService,
    Lazy<ISqlCommandAppService> sqlCommandAppService,
    Lazy<ILogger<ChannelService>> logger)
  {
    _projectService = projectService;
    _companyService = companyService;
    _enumerationService = enumerationService;
    _staticSettingService = staticSettingService;
    _productService = productService;
    _emailAlertService = emailAlertService;
    _textAlertService = textAlertService;
    _applicationUserService = applicationUserService;
    _sqlCommandAppService = sqlCommandAppService;
    _logger = logger;
  }

  #region DemoModule

  public async Task<Result<ProjectDTO>> AddProjectAsync(ProjectDTO projectDTO, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      return await _projectService.Value.AddProjectAsync(projectDTO, GetServiceHeader(serviceHeader), cts.Token).ConfigureAwait(false);
    }
    catch (OperationCanceledException ex)
    {
      _logger.Value.LogError(ex, "The operation was cancelled.");
      return Result<ProjectDTO>.Error(new[] { "The operation was cancelled." });
    }
    catch (Exception ex)
    {
      _logger.Value.LogError(ex, "An error occurred. {exceptionMessage}", ex.Message);
      return Result<ProjectDTO>.Error(new[] { ex.Message });
    }
  }

  public async Task<Result> UpdateProjectAsync(Guid projectId, ProjectDTO projectDTO, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      return await _projectService.Value.UpdateProjectAsync(projectId, projectDTO, GetServiceHeader(serviceHeader), cts.Token).ConfigureAwait(false);
    }
    catch (OperationCanceledException ex)
    {
      _logger.Value.LogError(ex, "The operation was cancelled.");
      return Result.Error(new[] { "The operation was cancelled." });
    }
    catch (Exception ex)
    {
      _logger.Value.LogError(ex, "An error occurred. {exceptionMessage}", ex.Message);
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result> DeleteProjectAsync(Guid projectId, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      return await _projectService.Value.DeleteProjectAsync(projectId, GetServiceHeader(serviceHeader), cts.Token).ConfigureAwait(false);
    }
    catch (OperationCanceledException ex)
    {
      _logger.Value.LogError(ex, "The operation was cancelled.");
      return Result.Error(new[] { "The operation was cancelled." });
    }
    catch (Exception ex)
    {
      _logger.Value.LogError(ex, "An error occurred. {exceptionMessage}", ex.Message);
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result<ProjectDTO>> FindProjectAsync(Guid projectId, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      return await _projectService.Value.FindProjectAsync(projectId, GetServiceHeader(serviceHeader), cts.Token).ConfigureAwait(false);
    }
    catch (OperationCanceledException ex)
    {
      _logger.Value.LogError(ex, "The operation was cancelled.");
      return Result<ProjectDTO>.Error(new[] { "The operation was cancelled." });
    }
    catch (Exception ex)
    {
      _logger.Value.LogError(ex, "An error occurred. {exceptionMessage}", ex.Message);
      return Result<ProjectDTO>.Error(new[] { ex.Message });
    }
  }

  public async Task<Result<List<ProjectDTO>>> FindProjectsAsync(ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      return await _projectService.Value.FindProjectsAsync(GetServiceHeader(serviceHeader), cts.Token).ConfigureAwait(false);
    }
    catch (OperationCanceledException ex)
    {
      _logger.Value.LogError(ex, "The operation was cancelled.");
      return Result<List<ProjectDTO>>.Error(new[] { "The operation was cancelled." });
    }
    catch (Exception ex)
    {
      _logger.Value.LogError(ex, "An error occurred. {exceptionMessage}", ex.Message);
      return Result<List<ProjectDTO>>.Error(new[] { ex.Message });
    }
  }

  public async Task<Result<PageCollectionInfo<ProjectDTO>>> GetProjectsWithFiltersAndInPageAsync(string? searchString, int pageNumber, int pageSize, string sortColumn, string sortDirection, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      return await _projectService.Value.GetProjectsWithFiltersAndInPageAsync(searchString, pageNumber, pageSize, sortColumn, sortDirection, GetServiceHeader(serviceHeader), cts.Token).ConfigureAwait(false);
    }
    catch (OperationCanceledException ex)
    {
      _logger.Value.LogError(ex, "The operation was cancelled.");
      return Result<PageCollectionInfo<ProjectDTO>>.Error(new[] { "The operation was cancelled." });
    }
    catch (Exception ex)
    {
      _logger.Value.LogError(ex, "An error occurred. {exceptionMessage}", ex.Message);
      return Result<PageCollectionInfo<ProjectDTO>>.Error(new[] { ex.Message });
    }
  }

  public async Task<Result> MarkProjectItemsCompleteAsync(Guid projectId, Guid itemId, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      return await _projectService.Value.MarkProjectItemsCompleteAsync(projectId, itemId, GetServiceHeader(serviceHeader), cts.Token).ConfigureAwait(false);
    }
    catch (OperationCanceledException ex)
    {
      _logger.Value.LogError(ex, "The operation was cancelled.");
      return Result.Error(new[] { "The operation was cancelled." });
    }
    catch (Exception ex)
    {
      _logger.Value.LogError(ex, "An error occurred. {exceptionMessage}", ex.Message);
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result> MarkProjectAllItemsCompleteAsync(Guid projectId, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      return await _projectService.Value.MarkProjectAllItemsCompleteAsync(projectId, GetServiceHeader(serviceHeader), cts.Token).ConfigureAwait(false);
    }
    catch (OperationCanceledException ex)
    {
      _logger.Value.LogError(ex, "The operation was cancelled.");
      return Result.Error(new[] { "The operation was cancelled." });
    }
    catch (Exception ex)
    {
      _logger.Value.LogError(ex, "An error occurred. {exceptionMessage}", ex.Message);
      return Result.Error(new[] { ex.Message });
    }
  }
  
  public async Task<Result<DashboardDTO>> FindDashboardsAsync(ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      return await _sqlCommandAppService.Value.FindDashboardsAsync(GetServiceHeader(serviceHeader), cts.Token).ConfigureAwait(false);
    }
    catch (OperationCanceledException ex)
    {
      _logger.Value.LogError(ex, "The operation was cancelled.");
      return Result.Error(new[] { "The operation was cancelled." });
    }
    catch (Exception ex)
    {
      _logger.Value.LogError(ex, "An error occurred. {exceptionMessage}", ex.Message);
      return Result.Error(new[] { ex.Message });
    }
  }

  #endregion

  #region AdministrationModule

  #region CompanyDTO

  public async Task<Result<CompanyDTO>> AddCompanyAsync(CompanyDTO companyDTO, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      return await _companyService.Value.AddCompanyAsync(companyDTO, GetServiceHeader(serviceHeader), cts.Token).ConfigureAwait(false);
    }
    catch (OperationCanceledException ex)
    {
      _logger.Value.LogError(ex, "The operation was cancelled.");
      return Result<CompanyDTO>.Error(new[] { "The operation was cancelled." });
    }
    catch (Exception ex)
    {
      _logger.Value.LogError(ex, "An error occurred. {exceptionMessage}", ex.Message);
      return Result<CompanyDTO>.Error(new[] { ex.Message });
    }
  }

  public async Task<Result> UpdateCompanyAsync(Guid companyId, CompanyDTO companyDTO, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      return await _companyService.Value.UpdateCompanyAsync(companyId, companyDTO, GetServiceHeader(serviceHeader), cts.Token).ConfigureAwait(false);
    }
    catch (OperationCanceledException ex)
    {
      _logger.Value.LogError(ex, "The operation was cancelled.");
      return Result.Error(new[] { "The operation was cancelled." });
    }
    catch (Exception ex)
    {
      _logger.Value.LogError(ex, "An error occurred. {exceptionMessage}", ex.Message);
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result> DeleteCompanyAsync(Guid companyId, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      return await _companyService.Value.DeleteCompanyAsync(companyId, GetServiceHeader(serviceHeader), cts.Token).ConfigureAwait(false);
    }
    catch (OperationCanceledException ex)
    {
      _logger.Value.LogError(ex, "The operation was cancelled.");
      return Result.Error(new[] { "The operation was cancelled." });
    }
    catch (Exception ex)
    {
      _logger.Value.LogError(ex, "An error occurred. {exceptionMessage}", ex.Message);
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result<CompanyDTO>> FindCompanyAsync(Guid companyId, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      return await _companyService.Value.FindCompanyAsync(companyId, GetServiceHeader(serviceHeader), cts.Token).ConfigureAwait(false);
    }
    catch (OperationCanceledException ex)
    {
      _logger.Value.LogError(ex, "The operation was cancelled.");
      return Result<CompanyDTO>.Error(new[] { "The operation was cancelled." });
    }
    catch (Exception ex)
    {
      _logger.Value.LogError(ex, "An error occurred. {exceptionMessage}", ex.Message);
      return Result<CompanyDTO>.Error(new[] { ex.Message });
    }
  }

  public async Task<Result<List<CompanyDTO>>> FindCompaniesAsync(ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      return await _companyService.Value.FindCompaniesAsync(GetServiceHeader(serviceHeader), cts.Token).ConfigureAwait(false);
    }
    catch (OperationCanceledException ex)
    {
      _logger.Value.LogError(ex, "The operation was cancelled.");
      return Result<List<CompanyDTO>>.Error(new[] { "The operation was cancelled." });
    }
    catch (Exception ex)
    {
      _logger.Value.LogError(ex, "An error occurred. {exceptionMessage}", ex.Message);
      return Result<List<CompanyDTO>>.Error(new[] { ex.Message });
    }
  }

  public async Task<Result<PageCollectionInfo<CompanyDTO>>> GetCompaniesWithFiltersAndInPageAsync(string? searchString, int pageNumber, int pageSize, string sortColumn, string sortDirection, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      return await _companyService.Value.GetCompaniesWithFiltersAndInPageAsync(searchString, pageNumber, pageSize, sortColumn, sortDirection, GetServiceHeader(serviceHeader), cts.Token).ConfigureAwait(false);
    }
    catch (OperationCanceledException ex)
    {
      _logger.Value.LogError(ex, "The operation was cancelled.");
      return Result<PageCollectionInfo<CompanyDTO>>.Error(new[] { "The operation was cancelled." });
    }
    catch (Exception ex)
    {
      _logger.Value.LogError(ex, "An error occurred. {exceptionMessage}", ex.Message);
      return Result<PageCollectionInfo<CompanyDTO>>.Error(new[] { ex.Message });
    }
  }

  #endregion

  #region StaticSettingDTO

  public async Task<Result<StaticSettingDTO>> AddStaticSettingAsync(StaticSettingDTO staticSettingDTO, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      return await _staticSettingService.Value.AddStaticSettingAsync(staticSettingDTO, GetServiceHeader(serviceHeader), cts.Token).ConfigureAwait(false);
    }
    catch (OperationCanceledException ex)
    {
      _logger.Value.LogError(ex, "The operation was cancelled.");
      return Result<StaticSettingDTO>.Error(new[] { "The operation was cancelled." });
    }
    catch (Exception ex)
    {
      _logger.Value.LogError(ex, "An error occurred. {exceptionMessage}", ex.Message);
      return Result<StaticSettingDTO>.Error(new[] { ex.Message });
    }
  }

  public async Task<Result> UpdateStaticSettingAsync(Guid staticSettingId, StaticSettingDTO staticSettingDTO, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      return await _staticSettingService.Value.UpdateStaticSettingAsync(staticSettingId, staticSettingDTO, GetServiceHeader(serviceHeader), cts.Token).ConfigureAwait(false);
    }
    catch (OperationCanceledException ex)
    {
      _logger.Value.LogError(ex, "The operation was cancelled.");
      return Result.Error(new[] { "The operation was cancelled." });
    }
    catch (Exception ex)
    {
      _logger.Value.LogError(ex, "An error occurred. {exceptionMessage}", ex.Message);
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result> DeleteStaticSettingAsync(Guid staticSettingId, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      return await _staticSettingService.Value.DeleteStaticSettingAsync(staticSettingId, GetServiceHeader(serviceHeader), cts.Token).ConfigureAwait(false);
    }
    catch (OperationCanceledException ex)
    {
      _logger.Value.LogError(ex, "The operation was cancelled.");
      return Result.Error(new[] { "The operation was cancelled." });
    }
    catch (Exception ex)
    {
      _logger.Value.LogError(ex, "An error occurred. {exceptionMessage}", ex.Message);
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result<StaticSettingDTO>> FindStaticSettingAsync(Guid staticSettingId, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      return await _staticSettingService.Value.FindStaticSettingAsync(staticSettingId, GetServiceHeader(serviceHeader), cts.Token).ConfigureAwait(false);
    }
    catch (OperationCanceledException ex)
    {
      _logger.Value.LogError(ex, "The operation was cancelled.");
      return Result<StaticSettingDTO>.Error(new[] { "The operation was cancelled." });
    }
    catch (Exception ex)
    {
      _logger.Value.LogError(ex, "An error occurred. {exceptionMessage}", ex.Message);
      return Result<StaticSettingDTO>.Error(new[] { ex.Message });
    }
  }
  
  public async Task<Result<StaticSettingDTO>> FindStaticSettingByKeyAsync(string key, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      return await _staticSettingService.Value.FindStaticSettingByKeyAsync(key, GetServiceHeader(serviceHeader), cts.Token).ConfigureAwait(false);
    }
    catch (OperationCanceledException ex)
    {
      _logger.Value.LogError(ex, "The operation was cancelled.");
      return Result<StaticSettingDTO>.Error(new[] { "The operation was cancelled." });
    }
    catch (Exception ex)
    {
      _logger.Value.LogError(ex, "An error occurred. {exceptionMessage}", ex.Message);
      return Result<StaticSettingDTO>.Error(new[] { ex.Message });
    }
  }

  public async Task<Result<List<StaticSettingDTO>>> FindStaticSettingsAsync(ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      return await _staticSettingService.Value.FindStaticSettingsAsync(GetServiceHeader(serviceHeader), cts.Token).ConfigureAwait(false);
    }
    catch (OperationCanceledException ex)
    {
      _logger.Value.LogError(ex, "The operation was cancelled.");
      return Result<List<StaticSettingDTO>>.Error(new[] { "The operation was cancelled." });
    }
    catch (Exception ex)
    {
      _logger.Value.LogError(ex, "An error occurred. {exceptionMessage}", ex.Message);
      return Result<List<StaticSettingDTO>>.Error(new[] { ex.Message });
    }
  }

  public async Task<Result<PageCollectionInfo<StaticSettingDTO>>> GetStaticSettingsWithFiltersAndInPageAsync(string? searchString, int pageNumber, int pageSize, string sortColumn, string sortDirection, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      return await _staticSettingService.Value.GetStaticSettingsWithFiltersAndInPageAsync(searchString, pageNumber, pageSize, sortColumn, sortDirection, GetServiceHeader(serviceHeader), cts.Token).ConfigureAwait(false);
    }
    catch (OperationCanceledException ex)
    {
      _logger.Value.LogError(ex, "The operation was cancelled.");
      return Result<PageCollectionInfo<StaticSettingDTO>>.Error(new[] { "The operation was cancelled." });
    }
    catch (Exception ex)
    {
      _logger.Value.LogError(ex, "An error occurred. {exceptionMessage}", ex.Message);
      return Result<PageCollectionInfo<StaticSettingDTO>>.Error(new[] { ex.Message });
    }
  }

  #endregion

  #region EnumerationDTO

  public async Task<Result<EnumerationDTO>> AddEnumerationAsync(EnumerationDTO enumerationDTO, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      return await _enumerationService.Value.AddEnumerationAsync(enumerationDTO, GetServiceHeader(serviceHeader), cts.Token).ConfigureAwait(false);
    }
    catch (OperationCanceledException ex)
    {
      _logger.Value.LogError(ex, "The operation was cancelled.");
      return Result<EnumerationDTO>.Error(new[] { "The operation was cancelled." });
    }
    catch (Exception ex)
    {
      _logger.Value.LogError(ex, "An error occurred. {exceptionMessage}", ex.Message);
      return Result<EnumerationDTO>.Error(new[] { ex.Message });
    }
  }

  public async Task<Result<bool>> BulkInsertEnumerationsAsync(List<EnumerationDTO> enumerationDTOs, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      return await _enumerationService.Value.BulkInsertEnumerationsAsync(enumerationDTOs, GetServiceHeader(serviceHeader), cts.Token).ConfigureAwait(false);
    }
    catch (OperationCanceledException ex)
    {
      _logger.Value.LogError(ex, "The operation was cancelled.");
      return Result<bool>.Error(new[] { "The operation was cancelled." });
    }
    catch (Exception ex)
    {
      _logger.Value.LogError(ex, "An error occurred. {exceptionMessage}", ex.Message);
      return Result<bool>.Error(new[] { ex.Message });
    }
  }

  public async Task<Result> UpdateEnumerationAsync(Guid enumerationId, EnumerationDTO enumerationDTO, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      return await _enumerationService.Value.UpdateEnumerationAsync(enumerationId, enumerationDTO, GetServiceHeader(serviceHeader), cts.Token).ConfigureAwait(false);
    }
    catch (OperationCanceledException ex)
    {
      _logger.Value.LogError(ex, "The operation was cancelled.");
      return Result.Error(new[] { "The operation was cancelled." });
    }
    catch (Exception ex)
    {
      _logger.Value.LogError(ex, "An error occurred. {exceptionMessage}", ex.Message);
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result> DeleteEnumerationAsync(Guid enumerationId, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      return await _enumerationService.Value.DeleteEnumerationAsync(enumerationId, GetServiceHeader(serviceHeader), cts.Token).ConfigureAwait(false);
    }
    catch (OperationCanceledException ex)
    {
      _logger.Value.LogError(ex, "The operation was cancelled.");
      return Result.Error(new[] { "The operation was cancelled." });
    }
    catch (Exception ex)
    {
      _logger.Value.LogError(ex, "An error occurred. {exceptionMessage}", ex.Message);
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result<EnumerationDTO>> FindEnumerationAsync(Guid enumerationId, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      return await _enumerationService.Value.FindEnumerationAsync(enumerationId, GetServiceHeader(serviceHeader), cts.Token).ConfigureAwait(false);
    }
    catch (OperationCanceledException ex)
    {
      _logger.Value.LogError(ex, "The operation was cancelled.");
      return Result<EnumerationDTO>.Error(new[] { "The operation was cancelled." });
    }
    catch (Exception ex)
    {
      _logger.Value.LogError(ex, "An error occurred. {exceptionMessage}", ex.Message);
      return Result<EnumerationDTO>.Error(new[] { ex.Message });
    }
  }

  public async Task<Result<List<EnumerationDTO>>> FindEnumerationsAsync(ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      return await _enumerationService.Value.FindEnumerationsAsync(GetServiceHeader(serviceHeader), cts.Token).ConfigureAwait(false);
    }
    catch (OperationCanceledException ex)
    {
      _logger.Value.LogError(ex, "The operation was cancelled.");
      return Result<List<EnumerationDTO>>.Error(new[] { "The operation was cancelled." });
    }
    catch (Exception ex)
    {
      _logger.Value.LogError(ex, "An error occurred. {exceptionMessage}", ex.Message);
      return Result<List<EnumerationDTO>>.Error(new[] { ex.Message });
    }
  }

  public async Task<Result<PageCollectionInfo<EnumerationDTO>>> GetEnumerationsWithFiltersAndInPageAsync(string? searchString, int pageNumber, int pageSize, string sortColumn, string sortDirection, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      return await _enumerationService.Value.GetEnumerationsWithFiltersAndInPageAsync(searchString, pageNumber, pageSize, sortColumn, sortDirection, GetServiceHeader(serviceHeader), cts.Token).ConfigureAwait(false);
    }
    catch (OperationCanceledException ex)
    {
      _logger.Value.LogError(ex, "The operation was cancelled.");
      return Result<PageCollectionInfo<EnumerationDTO>>.Error(new[] { "The operation was cancelled." });
    }
    catch (Exception ex)
    {
      _logger.Value.LogError(ex, "An error occurred. {exceptionMessage}", ex.Message);
      return Result<PageCollectionInfo<EnumerationDTO>>.Error(new[] { ex.Message });
    }
  }

  #endregion

  #endregion

  #region InventoryModule

  #region ProductDTO

  public async Task<Result<ProductDTO>> AddProductAsync(ProductDTO productDTO, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      return await _productService.Value.AddProductAsync(productDTO, GetServiceHeader(serviceHeader), cts.Token).ConfigureAwait(false);
    }
    catch (OperationCanceledException ex)
    {
      _logger.Value.LogError(ex, "The operation was cancelled.");
      return Result<ProductDTO>.Error(new[] { "The operation was cancelled." });
    }
    catch (Exception ex)
    {
      _logger.Value.LogError(ex, "An error occurred. {exceptionMessage}", ex.Message);
      return Result<ProductDTO>.Error(new[] { ex.Message });
    }
  }

  public async Task<Result<bool>> BulkInsertProductsAsync(List<ProductDTO> productDTOs, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      return await _productService.Value.BulkInsertProductsAsync(productDTOs, GetServiceHeader(serviceHeader), cts.Token).ConfigureAwait(false);
    }
    catch (OperationCanceledException ex)
    {
      _logger.Value.LogError(ex, "The operation was cancelled.");
      return Result<bool>.Error(new[] { "The operation was cancelled." });
    }
    catch (Exception ex)
    {
      _logger.Value.LogError(ex, "An error occurred. {exceptionMessage}", ex.Message);
      return Result<bool>.Error(new[] { ex.Message });
    }
  }

  public async Task<Result> UpdateProductAsync(Guid productId, ProductDTO productDTO, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      return await _productService.Value.UpdateProductAsync(productId, productDTO, GetServiceHeader(serviceHeader), cts.Token).ConfigureAwait(false);
    }
    catch (OperationCanceledException ex)
    {
      _logger.Value.LogError(ex, "The operation was cancelled.");
      return Result.Error(new[] { "The operation was cancelled." });
    }
    catch (Exception ex)
    {
      _logger.Value.LogError(ex, "An error occurred. {exceptionMessage}", ex.Message);
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result> DeleteProductAsync(Guid productId, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      return await _productService.Value.DeleteProductAsync(productId, GetServiceHeader(serviceHeader), cts.Token).ConfigureAwait(false);
    }
    catch (OperationCanceledException ex)
    {
      _logger.Value.LogError(ex, "The operation was cancelled.");
      return Result.Error(new[] { "The operation was cancelled." });
    }
    catch (Exception ex)
    {
      _logger.Value.LogError(ex, "An error occurred. {exceptionMessage}", ex.Message);
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result<ProductDTO>> FindProductAsync(Guid productId, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      return await _productService.Value.FindProductAsync(productId, GetServiceHeader(serviceHeader), cts.Token).ConfigureAwait(false);
    }
    catch (OperationCanceledException ex)
    {
      _logger.Value.LogError(ex, "The operation was cancelled.");
      return Result<ProductDTO>.Error(new[] { "The operation was cancelled." });
    }
    catch (Exception ex)
    {
      _logger.Value.LogError(ex, "An error occurred. {exceptionMessage}", ex.Message);
      return Result<ProductDTO>.Error(new[] { ex.Message });
    }
  }

  public async Task<Result<List<ProductDTO>>> FindProductsAsync(ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      return await _productService.Value.FindProductsAsync(GetServiceHeader(serviceHeader), cts.Token).ConfigureAwait(false);
    }
    catch (OperationCanceledException ex)
    {
      _logger.Value.LogError(ex, "The operation was cancelled.");
      return Result<List<ProductDTO>>.Error(new[] { "The operation was cancelled." });
    }
    catch (Exception ex)
    {
      _logger.Value.LogError(ex, "An error occurred. {exceptionMessage}", ex.Message);
      return Result<List<ProductDTO>>.Error(new[] { ex.Message });
    }
  }

  public async Task<Result<PageCollectionInfo<ProductDTO>>> GetProductsWithFiltersAndInPageAsync(string? searchString, int pageNumber, int pageSize, string sortColumn, string sortDirection, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      return await _productService.Value.GetProductsWithFiltersAndInPageAsync(searchString, pageNumber, pageSize, sortColumn, sortDirection, GetServiceHeader(serviceHeader), cts.Token).ConfigureAwait(false);
    }
    catch (OperationCanceledException ex)
    {
      _logger.Value.LogError(ex, "The operation was cancelled.");
      return Result<PageCollectionInfo<ProductDTO>>.Error(new[] { "The operation was cancelled." });
    }
    catch (Exception ex)
    {
      _logger.Value.LogError(ex, "An error occurred. {exceptionMessage}", ex.Message);
      return Result<PageCollectionInfo<ProductDTO>>.Error(new[] { ex.Message });
    }
  }

  #endregion

  #endregion

  #region UserManagementModule

  #region ApplicationUserDTO

  public async Task<Result<ApplicationUserDTO>> AddApplicationUserAsync(ApplicationUserDTO applicationUserDTO, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      return await _applicationUserService.Value.AddApplicationUserAsync(applicationUserDTO, GetServiceHeader(serviceHeader), cts.Token).ConfigureAwait(false);
    }
    catch (OperationCanceledException ex)
    {
      _logger.Value.LogError(ex, "The operation was cancelled.");
      return Result<ApplicationUserDTO>.Error(new[] { "The operation was cancelled." });
    }
    catch (Exception ex)
    {
      _logger.Value.LogError(ex, "An error occurred. {exceptionMessage}", ex.Message);
      return Result<ApplicationUserDTO>.Error(new[] { ex.Message });
    }
  }

  public async Task<Result<(IdentityResult, string UserId, string Code)>> CreateApplicationUserAsync(ApplicationUserDTO applicationUserDTO, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      return await _applicationUserService.Value.CreateApplicationUserAsync(applicationUserDTO, GetServiceHeader(serviceHeader), cts.Token).ConfigureAwait(false);
    }
    catch (OperationCanceledException ex)
    {
      _logger.Value.LogError(ex, "The operation was cancelled.");
      return Result<(IdentityResult, string, string)>.Error(new[] { "The operation was cancelled." });
    }
    catch (Exception ex)
    {
      _logger.Value.LogError(ex, "An error occurred. {exceptionMessage}", ex.Message);
      return Result<(IdentityResult, string, string)>.Error(new[] { ex.Message });
    }
  }
  
  public async Task<Result<IdentityResult>> ConfirmApplicationUserEmailAsync(string userId, string code, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      return await _applicationUserService.Value.ConfirmApplicationUserEmailAsync(userId, code, GetServiceHeader(serviceHeader), cts.Token).ConfigureAwait(false);
    }
    catch (OperationCanceledException ex)
    {
      _logger.Value.LogError(ex, "The operation was cancelled.");
      return Result.Error(new[] { "The operation was cancelled." });
    }
    catch (Exception ex)
    {
      _logger.Value.LogError(ex, "An error occurred. {exceptionMessage}", ex.Message);
      return Result.Error(new[] { ex.Message });
    }
  }
  
  public async Task<Result<Microsoft.AspNetCore.Identity.SignInResult>> LoginApplicationUserAsync(AccountLoginBindingModel accountLoginBindingModel, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      return await _applicationUserService.Value.LoginApplicationUserAsync(accountLoginBindingModel, GetServiceHeader(serviceHeader), cts.Token).ConfigureAwait(false);
    }
    catch (OperationCanceledException ex)
    {
      _logger.Value.LogError(ex, "The operation was cancelled.");
      return Result<Microsoft.AspNetCore.Identity.SignInResult>.Error(new[] { "The operation was cancelled." });
    }
    catch (Exception ex)
    {
      _logger.Value.LogError(ex, "An error occurred. {exceptionMessage}", ex.Message);
      return Result<Microsoft.AspNetCore.Identity.SignInResult>.Error(new[] { ex.Message });
    }
  }

  public async Task<Result<bool>> BulkInsertApplicationUsersAsync(List<ApplicationUserDTO> applicationUserDTOs, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      return await _applicationUserService.Value.BulkInsertApplicationUsersAsync(applicationUserDTOs, GetServiceHeader(serviceHeader), cts.Token).ConfigureAwait(false);
    }
    catch (OperationCanceledException ex)
    {
      _logger.Value.LogError(ex, "The operation was cancelled.");
      return Result<bool>.Error(new[] { "The operation was cancelled." });
    }
    catch (Exception ex)
    {
      _logger.Value.LogError(ex, "An error occurred. {exceptionMessage}", ex.Message);
      return Result<bool>.Error(new[] { ex.Message });
    }
  }

  public async Task<Result> UpdateApplicationUserAsync(Guid applicationUserId, ApplicationUserDTO applicationUserDTO, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      return await _applicationUserService.Value.UpdateApplicationUserAsync(applicationUserId, applicationUserDTO, GetServiceHeader(serviceHeader), cts.Token).ConfigureAwait(false);
    }
    catch (OperationCanceledException ex)
    {
      _logger.Value.LogError(ex, "The operation was cancelled.");
      return Result.Error(new[] { "The operation was cancelled." });
    }
    catch (Exception ex)
    {
      _logger.Value.LogError(ex, "An error occurred. {exceptionMessage}", ex.Message);
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result> DeleteApplicationUserAsync(Guid applicationUserId, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      return await _applicationUserService.Value.DeleteApplicationUserAsync(applicationUserId, GetServiceHeader(serviceHeader), cts.Token).ConfigureAwait(false);
    }
    catch (OperationCanceledException ex)
    {
      _logger.Value.LogError(ex, "The operation was cancelled.");
      return Result.Error(new[] { "The operation was cancelled." });
    }
    catch (Exception ex)
    {
      _logger.Value.LogError(ex, "An error occurred. {exceptionMessage}", ex.Message);
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result<ApplicationUserDTO>> FindApplicationUserAsync(Guid applicationUserId, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      return await _applicationUserService.Value.FindApplicationUserAsync(applicationUserId, GetServiceHeader(serviceHeader), cts.Token).ConfigureAwait(false);
    }
    catch (OperationCanceledException ex)
    {
      _logger.Value.LogError(ex, "The operation was cancelled.");
      return Result<ApplicationUserDTO>.Error(new[] { "The operation was cancelled." });
    }
    catch (Exception ex)
    {
      _logger.Value.LogError(ex, "An error occurred. {exceptionMessage}", ex.Message);
      return Result<ApplicationUserDTO>.Error(new[] { ex.Message });
    }
  }

  public async Task<Result<List<ApplicationUserDTO>>> FindApplicationUsersAsync(ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      return await _applicationUserService.Value.FindApplicationUsersAsync(GetServiceHeader(serviceHeader), cts.Token).ConfigureAwait(false);
    }
    catch (OperationCanceledException ex)
    {
      _logger.Value.LogError(ex, "The operation was cancelled.");
      return Result<List<ApplicationUserDTO>>.Error(new[] { "The operation was cancelled." });
    }
    catch (Exception ex)
    {
      _logger.Value.LogError(ex, "An error occurred. {exceptionMessage}", ex.Message);
      return Result<List<ApplicationUserDTO>>.Error(new[] { ex.Message });
    }
  }

  public async Task<Result<PageCollectionInfo<ApplicationUserDTO>>> GetApplicationUsersWithFiltersAndInPageAsync(string? searchString, int pageNumber, int pageSize, string sortColumn, string sortDirection, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      return await _applicationUserService.Value.GetApplicationUsersWithFiltersAndInPageAsync(searchString, pageNumber, pageSize, sortColumn, sortDirection, GetServiceHeader(serviceHeader), cts.Token).ConfigureAwait(false);
    }
    catch (OperationCanceledException ex)
    {
      _logger.Value.LogError(ex, "The operation was cancelled.");
      return Result<PageCollectionInfo<ApplicationUserDTO>>.Error(new[] { "The operation was cancelled." });
    }
    catch (Exception ex)
    {
      _logger.Value.LogError(ex, "An error occurred. {exceptionMessage}", ex.Message);
      return Result<PageCollectionInfo<ApplicationUserDTO>>.Error(new[] { ex.Message });
    }
  }

  #endregion

  #endregion

  #region MessagingModule

  #region EmailAlertDTO

  public async Task<Result<EmailAlertDTO>> AddEmailAlertAsync(EmailAlertDTO emailAlertDTO, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      return await _emailAlertService.Value.AddEmailAlertAsync(emailAlertDTO, GetServiceHeader(serviceHeader), cts.Token).ConfigureAwait(false);
    }
    catch (OperationCanceledException ex)
    {
      _logger.Value.LogError(ex, "The operation was cancelled.");
      return Result<EmailAlertDTO>.Error(new[] { "The operation was cancelled." });
    }
    catch (Exception ex)
    {
      _logger.Value.LogError(ex, "An error occurred. {exceptionMessage}", ex.Message);
      return Result<EmailAlertDTO>.Error(new[] { ex.Message });
    }
  }

  public async Task<Result<bool>> BulkInsertEmailAlertsAsync(List<EmailAlertDTO> emailAlertDTOs, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      return await _emailAlertService.Value.BulkInsertEmailAlertsAsync(emailAlertDTOs, GetServiceHeader(serviceHeader), cts.Token).ConfigureAwait(false);
    }
    catch (OperationCanceledException ex)
    {
      _logger.Value.LogError(ex, "The operation was cancelled.");
      return Result<bool>.Error(new[] { "The operation was cancelled." });
    }
    catch (Exception ex)
    {
      _logger.Value.LogError(ex, "An error occurred. {exceptionMessage}", ex.Message);
      return Result<bool>.Error(new[] { ex.Message });
    }
  }

  public async Task<Result> UpdateEmailAlertAsync(Guid emailAlertId, EmailAlertDTO emailAlertDTO, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      return await _emailAlertService.Value.UpdateEmailAlertAsync(emailAlertId, emailAlertDTO, GetServiceHeader(serviceHeader), cts.Token).ConfigureAwait(false);
    }
    catch (OperationCanceledException ex)
    {
      _logger.Value.LogError(ex, "The operation was cancelled.");
      return Result.Error(new[] { "The operation was cancelled." });
    }
    catch (Exception ex)
    {
      _logger.Value.LogError(ex, "An error occurred. {exceptionMessage}", ex.Message);
      return Result.Error(new[] { ex.Message });
    }
  }
  
  public async Task<Result> MarkQueuedEmailAlertAsync(Guid emailAlertId, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      return await _emailAlertService.Value.MarkQueuedEmailAlertAsync(emailAlertId, GetServiceHeader(serviceHeader), cts.Token).ConfigureAwait(false);
    }
    catch (OperationCanceledException ex)
    {
      _logger.Value.LogError(ex, "The operation was cancelled.");
      return Result.Error(new[] { "The operation was cancelled." });
    }
    catch (Exception ex)
    {
      _logger.Value.LogError(ex, "An error occurred. {exceptionMessage}", ex.Message);
      return Result.Error(new[] { ex.Message });
    }
  }
  
  public async Task<Result> MarkDeliveredEmailAlertAsync(Guid emailAlertId, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      return await _emailAlertService.Value.MarkDeliveredEmailAlertAsync(emailAlertId, GetServiceHeader(serviceHeader), cts.Token).ConfigureAwait(false);
    }
    catch (OperationCanceledException ex)
    {
      _logger.Value.LogError(ex, "The operation was cancelled.");
      return Result.Error(new[] { "The operation was cancelled." });
    }
    catch (Exception ex)
    {
      _logger.Value.LogError(ex, "An error occurred. {exceptionMessage}", ex.Message);
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result> DeleteEmailAlertAsync(Guid emailAlertId, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      return await _emailAlertService.Value.DeleteEmailAlertAsync(emailAlertId, GetServiceHeader(serviceHeader), cts.Token).ConfigureAwait(false);
    }
    catch (OperationCanceledException ex)
    {
      _logger.Value.LogError(ex, "The operation was cancelled.");
      return Result.Error(new[] { "The operation was cancelled." });
    }
    catch (Exception ex)
    {
      _logger.Value.LogError(ex, "An error occurred. {exceptionMessage}", ex.Message);
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result<EmailAlertDTO>> FindEmailAlertAsync(Guid emailAlertId, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      return await _emailAlertService.Value.FindEmailAlertAsync(emailAlertId, GetServiceHeader(serviceHeader), cts.Token).ConfigureAwait(false);
    }
    catch (OperationCanceledException ex)
    {
      _logger.Value.LogError(ex, "The operation was cancelled.");
      return Result<EmailAlertDTO>.Error(new[] { "The operation was cancelled." });
    }
    catch (Exception ex)
    {
      _logger.Value.LogError(ex, "An error occurred. {exceptionMessage}", ex.Message);
      return Result<EmailAlertDTO>.Error(new[] { ex.Message });
    }
  }

  public async Task<Result<List<EmailAlertDTO>>> FindEmailAlertsAsync(ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      return await _emailAlertService.Value.FindEmailAlertsAsync(GetServiceHeader(serviceHeader), cts.Token).ConfigureAwait(false);
    }
    catch (OperationCanceledException ex)
    {
      _logger.Value.LogError(ex, "The operation was cancelled.");
      return Result<List<EmailAlertDTO>>.Error(new[] { "The operation was cancelled." });
    }
    catch (Exception ex)
    {
      _logger.Value.LogError(ex, "An error occurred. {exceptionMessage}", ex.Message);
      return Result<List<EmailAlertDTO>>.Error(new[] { ex.Message });
    }
  }

  public async Task<Result<PageCollectionInfo<EmailAlertDTO>>> GetEmailAlertsWithFiltersAndInPageAsync(string? searchString, int pageNumber, int pageSize, string sortColumn, string sortDirection, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      return await _emailAlertService.Value.GetEmailAlertsWithFiltersAndInPageAsync(searchString, pageNumber, pageSize, sortColumn, sortDirection, GetServiceHeader(serviceHeader), cts.Token).ConfigureAwait(false);
    }
    catch (OperationCanceledException ex)
    {
      _logger.Value.LogError(ex, "The operation was cancelled.");
      return Result<PageCollectionInfo<EmailAlertDTO>>.Error(new[] { "The operation was cancelled." });
    }
    catch (Exception ex)
    {
      _logger.Value.LogError(ex, "An error occurred. {exceptionMessage}", ex.Message);
      return Result<PageCollectionInfo<EmailAlertDTO>>.Error(new[] { ex.Message });
    }
  }
  
  public async Task<Result<List<EmailAlertDTO>>> GetEmailAlertsByDLRStatusAndWithFiltersAndInPageAsync(byte[] dlrStatuses, string? searchString, int pageSize, string sortColumn, string sortDirection, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      return await _emailAlertService.Value.GetEmailAlertsByDLRStatusAndWithFiltersAndInPageAsync(dlrStatuses, searchString, pageSize, sortColumn, sortDirection, GetServiceHeader(serviceHeader), cts.Token).ConfigureAwait(false);
    }
    catch (OperationCanceledException ex)
    {
      _logger.Value.LogError(ex, "The operation was cancelled.");
      return Result<List<EmailAlertDTO>>.Error(new[] { "The operation was cancelled." });
    }
    catch (Exception ex)
    {
      _logger.Value.LogError(ex, "An error occurred. {exceptionMessage}", ex.Message);
      return Result<List<EmailAlertDTO>>.Error(new[] { ex.Message });
    }
  }

  #endregion

  #region TextAlertDTO

  public async Task<Result<TextAlertDTO>> AddTextAlertAsync(TextAlertDTO textAlertDTO, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      return await _textAlertService.Value.AddTextAlertAsync(textAlertDTO, GetServiceHeader(serviceHeader), cts.Token).ConfigureAwait(false);
    }
    catch (OperationCanceledException ex)
    {
      _logger.Value.LogError(ex, "The operation was cancelled.");
      return Result<TextAlertDTO>.Error(new[] { "The operation was cancelled." });
    }
    catch (Exception ex)
    {
      _logger.Value.LogError(ex, "An error occurred. {exceptionMessage}", ex.Message);
      return Result<TextAlertDTO>.Error(new[] { ex.Message });
    }
  }

  public async Task<Result<bool>> BulkInsertTextAlertsAsync(List<TextAlertDTO> textAlertDTOs, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      return await _textAlertService.Value.BulkInsertTextAlertsAsync(textAlertDTOs, GetServiceHeader(serviceHeader), cts.Token).ConfigureAwait(false);
    }
    catch (OperationCanceledException ex)
    {
      _logger.Value.LogError(ex, "The operation was cancelled.");
      return Result<bool>.Error(new[] { "The operation was cancelled." });
    }
    catch (Exception ex)
    {
      _logger.Value.LogError(ex, "An error occurred. {exceptionMessage}", ex.Message);
      return Result<bool>.Error(new[] { ex.Message });
    }
  }

  public async Task<Result> UpdateTextAlertAsync(Guid textAlertId, TextAlertDTO textAlertDTO, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      return await _textAlertService.Value.UpdateTextAlertAsync(textAlertId, textAlertDTO, GetServiceHeader(serviceHeader), cts.Token).ConfigureAwait(false);
    }
    catch (OperationCanceledException ex)
    {
      _logger.Value.LogError(ex, "The operation was cancelled.");
      return Result.Error(new[] { "The operation was cancelled." });
    }
    catch (Exception ex)
    {
      _logger.Value.LogError(ex, "An error occurred. {exceptionMessage}", ex.Message);
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result> DeleteTextAlertAsync(Guid textAlertId, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      return await _textAlertService.Value.DeleteTextAlertAsync(textAlertId, GetServiceHeader(serviceHeader), cts.Token).ConfigureAwait(false);
    }
    catch (OperationCanceledException ex)
    {
      _logger.Value.LogError(ex, "The operation was cancelled.");
      return Result.Error(new[] { "The operation was cancelled." });
    }
    catch (Exception ex)
    {
      _logger.Value.LogError(ex, "An error occurred. {exceptionMessage}", ex.Message);
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result<TextAlertDTO>> FindTextAlertAsync(Guid textAlertId, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      return await _textAlertService.Value.FindTextAlertAsync(textAlertId, GetServiceHeader(serviceHeader), cts.Token).ConfigureAwait(false);
    }
    catch (OperationCanceledException ex)
    {
      _logger.Value.LogError(ex, "The operation was cancelled.");
      return Result<TextAlertDTO>.Error(new[] { "The operation was cancelled." });
    }
    catch (Exception ex)
    {
      _logger.Value.LogError(ex, "An error occurred. {exceptionMessage}", ex.Message);
      return Result<TextAlertDTO>.Error(new[] { ex.Message });
    }
  }

  public async Task<Result<List<TextAlertDTO>>> FindTextAlertsAsync(ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      return await _textAlertService.Value.FindTextAlertsAsync(GetServiceHeader(serviceHeader), cts.Token).ConfigureAwait(false);
    }
    catch (OperationCanceledException ex)
    {
      _logger.Value.LogError(ex, "The operation was cancelled.");
      return Result<List<TextAlertDTO>>.Error(new[] { "The operation was cancelled." });
    }
    catch (Exception ex)
    {
      _logger.Value.LogError(ex, "An error occurred. {exceptionMessage}", ex.Message);
      return Result<List<TextAlertDTO>>.Error(new[] { ex.Message });
    }
  }

  public async Task<Result<PageCollectionInfo<TextAlertDTO>>> GetTextAlertsWithFiltersAndInPageAsync(string? searchString, int pageNumber, int pageSize, string sortColumn, string sortDirection, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      return await _textAlertService.Value.GetTextAlertsWithFiltersAndInPageAsync(searchString, pageNumber, pageSize, sortColumn, sortDirection, GetServiceHeader(serviceHeader), cts.Token).ConfigureAwait(false);
    }
    catch (OperationCanceledException ex)
    {
      _logger.Value.LogError(ex, "The operation was cancelled.");
      return Result<PageCollectionInfo<TextAlertDTO>>.Error(new[] { "The operation was cancelled." });
    }
    catch (Exception ex)
    {
      _logger.Value.LogError(ex, "An error occurred. {exceptionMessage}", ex.Message);
      return Result<PageCollectionInfo<TextAlertDTO>>.Error(new[] { ex.Message });
    }
  }

  #endregion

  #endregion

  #region Helpers

  private static ServiceHeader GetServiceHeader(ServiceHeader? serviceHeader)
  {
    return serviceHeader ?? new ServiceHeader();
  }

  #endregion
}

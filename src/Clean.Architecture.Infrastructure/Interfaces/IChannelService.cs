using Clean.Architecture.Application.DTO;
using Clean.Architecture.Application.DTO.AdministrationModule.CompanyAggregate;
using Clean.Architecture.Application.DTO.AdministrationModule.EnumerationAggregate;
using Clean.Architecture.Application.DTO.AdministrationModule.StaticSettingAggregate;
using Clean.Architecture.Application.DTO.InventoryModule.ProductAggregate;
using Clean.Architecture.Application.DTO.MessagingModule.EmailAlertAggregate;
using Clean.Architecture.Application.DTO.MessagingModule.TextAlertAggregate;
using Clean.Architecture.Application.DTO.ProjectAggregate;
using Clean.Architecture.Application.DTO.UserManagementModule.ApplicationUserAggregate;

using Microsoft.AspNetCore.Identity;

namespace Clean.Architecture.Infrastructure.Interfaces;

public interface IChannelService
{
  #region ProjectDTO

  Task<Result<ProjectDTO>> AddProjectAsync(ProjectDTO projectDTO, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default);

  Task<Result> UpdateProjectAsync(Guid projectId, ProjectDTO projectDTO, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default);

  Task<Result> DeleteProjectAsync(Guid projectId, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default);

  Task<Result<ProjectDTO>> FindProjectAsync(Guid projectId, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default);

  Task<Result<List<ProjectDTO>>> FindProjectsAsync(ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default);

  Task<Result<PageCollectionInfo<ProjectDTO>>> GetProjectsWithFiltersAndInPageAsync(string? searchString, int pageNumber, int pageSize, string sortColumn, string sortDirection, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default);

  Task<Result> MarkProjectItemsCompleteAsync(Guid projectId, Guid itemId, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default);

  Task<Result> MarkProjectAllItemsCompleteAsync(Guid projectId, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default);

  Task<Result<DashboardDTO>> FindDashboardsAsync(ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default);

  #endregion

  #region AdministrationModule

  #region CompanyDTO

  Task<Result<CompanyDTO>> AddCompanyAsync(CompanyDTO companyDTO, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default);

  Task<Result> UpdateCompanyAsync(Guid companyId, CompanyDTO companyDTO, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default);

  Task<Result> DeleteCompanyAsync(Guid companyId, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default);

  Task<Result<CompanyDTO>> FindCompanyAsync(Guid companyId, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default);

  Task<Result<List<CompanyDTO>>> FindCompaniesAsync(ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default);

  Task<Result<PageCollectionInfo<CompanyDTO>>> GetCompaniesWithFiltersAndInPageAsync(string? searchString, int pageNumber, int pageSize, string sortColumn, string sortDirection, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default);

  #endregion

  #region StaticSettingDTO

  Task<Result<StaticSettingDTO>> AddStaticSettingAsync(StaticSettingDTO staticSettingDTO, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default);

  Task<Result> UpdateStaticSettingAsync(Guid staticSettingId, StaticSettingDTO staticSettingDTO, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default);

  Task<Result> DeleteStaticSettingAsync(Guid staticSettingId, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default);

  Task<Result<StaticSettingDTO>> FindStaticSettingAsync(Guid staticSettingId, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default);

  Task<Result<StaticSettingDTO>> FindStaticSettingByKeyAsync(string key, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default);

  Task<Result<List<StaticSettingDTO>>> FindStaticSettingsAsync(ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default);

  Task<Result<PageCollectionInfo<StaticSettingDTO>>> GetStaticSettingsWithFiltersAndInPageAsync(string? searchString, int pageNumber, int pageSize, string sortColumn, string sortDirection, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default);

  #endregion

  #region EnumerationDTO

  Task<Result<EnumerationDTO>> AddEnumerationAsync(EnumerationDTO enumerationDTO, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default);

  Task<Result<bool>> BulkInsertEnumerationsAsync(List<EnumerationDTO> enumerationDTOs, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default);

  Task<Result> UpdateEnumerationAsync(Guid enumerationId, EnumerationDTO enumerationDTO, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default);

  Task<Result> DeleteEnumerationAsync(Guid enumerationId, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default);

  Task<Result<EnumerationDTO>> FindEnumerationAsync(Guid enumerationId, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default);

  Task<Result<List<EnumerationDTO>>> FindEnumerationsAsync(ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default);

  Task<Result<PageCollectionInfo<EnumerationDTO>>> GetEnumerationsWithFiltersAndInPageAsync(string? searchString, int pageNumber, int pageSize, string sortColumn, string sortDirection, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default);

  #endregion

  #endregion

  #region InventoryModule

  #region ProductDTO

  Task<Result<ProductDTO>> AddProductAsync(ProductDTO productDTO, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default);

  Task<Result<bool>> BulkInsertProductsAsync(List<ProductDTO> productDTOs, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default);

  Task<Result> UpdateProductAsync(Guid productId, ProductDTO productDTO, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default);

  Task<Result> DeleteProductAsync(Guid productId, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default);

  Task<Result<ProductDTO>> FindProductAsync(Guid productId, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default);

  Task<Result<List<ProductDTO>>> FindProductsAsync(ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default);

  Task<Result<PageCollectionInfo<ProductDTO>>> GetProductsWithFiltersAndInPageAsync(string? searchString, int pageNumber, int pageSize, string sortColumn, string sortDirection, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default);

  #endregion

  #endregion

  #region MessagingModule

  #region EmailAlertDTO

  Task<Result<EmailAlertDTO>> AddEmailAlertAsync(EmailAlertDTO emailAlertDTO, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default);

  Task<Result<bool>> BulkInsertEmailAlertsAsync(List<EmailAlertDTO> emailAlertDTOs, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default);

  Task<Result> UpdateEmailAlertAsync(Guid emailAlertId, EmailAlertDTO emailAlertDTO, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default);

  Task<Result> MarkQueuedEmailAlertAsync(Guid emailAlertId, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default);

  Task<Result> MarkDeliveredEmailAlertAsync(Guid emailAlertId, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default);

  Task<Result> DeleteEmailAlertAsync(Guid emailAlertId, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default);

  Task<Result<EmailAlertDTO>> FindEmailAlertAsync(Guid emailAlertId, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default);

  Task<Result<List<EmailAlertDTO>>> FindEmailAlertsAsync(ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default);

  Task<Result<PageCollectionInfo<EmailAlertDTO>>> GetEmailAlertsWithFiltersAndInPageAsync(string? searchString, int pageNumber, int pageSize, string sortColumn, string sortDirection, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default);

  Task<Result<List<EmailAlertDTO>>> GetEmailAlertsByDLRStatusAndWithFiltersAndInPageAsync(byte[] dlrStatuses, string? searchString, int pageSize, string sortColumn, string sortDirection, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default);

  #endregion

  #region TextAlertDTO

  Task<Result<TextAlertDTO>> AddTextAlertAsync(TextAlertDTO textAlertDTO, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default);

  Task<Result<bool>> BulkInsertTextAlertsAsync(List<TextAlertDTO> textAlertDTOs, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default);

  Task<Result> UpdateTextAlertAsync(Guid textAlertId, TextAlertDTO textAlertDTO, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default);

  Task<Result> DeleteTextAlertAsync(Guid textAlertId, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default);

  Task<Result<TextAlertDTO>> FindTextAlertAsync(Guid textAlertId, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default);

  Task<Result<List<TextAlertDTO>>> FindTextAlertsAsync(ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default);

  Task<Result<PageCollectionInfo<TextAlertDTO>>> GetTextAlertsWithFiltersAndInPageAsync(string? searchString, int pageNumber, int pageSize, string sortColumn, string sortDirection, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default);

  #endregion

  #endregion

  #region UserManagementModule

  #region ApplicationUserDTO

  Task<Result<ApplicationUserDTO>> AddApplicationUserAsync(ApplicationUserDTO applicationUserDTO, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default);

  Task<Result<(IdentityResult, string UserId, string Code)>> CreateApplicationUserAsync(ApplicationUserDTO applicationUserDTO, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default);

  Task<Result<IdentityResult>> ConfirmApplicationUserEmailAsync(string userId, string code, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default);

  Task<Result<Microsoft.AspNetCore.Identity.SignInResult>> LoginApplicationUserAsync(AccountLoginBindingModel accountLoginBindingModel, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default);

  Task<Result<bool>> BulkInsertApplicationUsersAsync(List<ApplicationUserDTO> applicationUserDTOs, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default);

  Task<Result> UpdateApplicationUserAsync(Guid applicationUserId, ApplicationUserDTO applicationUserDTO, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default);

  Task<Result> DeleteApplicationUserAsync(Guid applicationUserId, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default);

  Task<Result<ApplicationUserDTO>> FindApplicationUserAsync(Guid applicationUserId, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default);

  Task<Result<List<ApplicationUserDTO>>> FindApplicationUsersAsync(ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default);

  Task<Result<PageCollectionInfo<ApplicationUserDTO>>> GetApplicationUsersWithFiltersAndInPageAsync(string? searchString, int pageNumber, int pageSize, string sortColumn, string sortDirection, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default);

  #endregion

  #endregion
}

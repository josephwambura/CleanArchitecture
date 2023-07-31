using Ardalis.Result;

using Clean.Architecture.Application.DTO.ProjectAggregate;
using Clean.Architecture.SharedKernel;
using Clean.Architecture.SharedKernel.Utils;

namespace Clean.Architecture.Application.Interfaces;

public interface IProjectService
{
  Task<Result<ProjectDTO>> AddProjectAsync(ProjectDTO projectDTO, ServiceHeader serviceHeader, CancellationToken cancellationToken = default);

  Task<Result> UpdateProjectAsync(Guid projectId, ProjectDTO projectDTO, ServiceHeader serviceHeader, CancellationToken cancellationToken = default);

  Task<Result> DeleteProjectAsync(Guid projectId, ServiceHeader serviceHeader, CancellationToken cancellationToken = default);

  Task<Result<ProjectDTO>> FindProjectAsync(Guid projectId, ServiceHeader serviceHeader, CancellationToken cancellationToken = default);

  Task<Result<List<ProjectDTO>>> FindProjectsAsync(ServiceHeader serviceHeader, CancellationToken cancellationToken = default);

  Task<Result<PageCollectionInfo<ProjectDTO>>> GetProjectsWithFiltersAndInPageAsync(string? searchString, int pageNumber, int pageSize, string sortColumn, string sortDirection, ServiceHeader serviceHeader, CancellationToken cancellationToken = default);

  Task<Result> MarkProjectItemsCompleteAsync(Guid projectId, Guid itemId, ServiceHeader serviceHeader, CancellationToken cancellationToken = default);

  Task<Result> MarkProjectAllItemsCompleteAsync(Guid projectId, ServiceHeader serviceHeader, CancellationToken cancellationToken = default);
}

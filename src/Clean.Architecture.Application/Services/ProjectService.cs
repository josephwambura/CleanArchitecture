using Ardalis.Result;

using Clean.Architecture.Application.DTO.ProjectAggregate;
using Clean.Architecture.Application.Interfaces;
using Clean.Architecture.Core.ProjectAggregate;
using Clean.Architecture.Core.ProjectAggregate.Events;
using Clean.Architecture.Core.ProjectAggregate.Specifications;
using Clean.Architecture.SharedKernel;
using Clean.Architecture.SharedKernel.Extensions;
using Clean.Architecture.SharedKernel.Interfaces;
using Clean.Architecture.SharedKernel.Utils;

using MediatR;

namespace Clean.Architecture.Application.Services;

public class ProjectService : IProjectService
{
  private readonly IUnitOfWork _unitOfWork;
  private readonly IReadRepository<Project>? _readRepository;
  private readonly IRepository<Project>? _repository;
  private readonly IMediator _mediator;

  public ProjectService(IUnitOfWork unitOfWork, IMediator mediator)
  {
    _unitOfWork = unitOfWork;
    _mediator = mediator;

    _readRepository = _unitOfWork.GetIReadRepository<Project>((byte)DbConnectionContext.LIVE);
    _repository = _unitOfWork.GetIRepository<Project>((byte)DbConnectionContext.LIVE);
  }

  public async Task<Result<ProjectDTO>> AddProjectAsync(ProjectDTO projectDTO, ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      var aggregateToAdd = ProjectFactory.CreateProject(projectDTO.Name, projectDTO.Priority, (byte)RecordStatus.Approved, serviceHeader);

      var result = await _repository!.AddAsync(aggregateToAdd, cts.Token);
      await _unitOfWork.SaveAsync();
      var domainEvent = new ProjectAddedEvent(result);
      await _mediator.Publish(domainEvent, cts.Token).ConfigureAwait(false);
      return Result.Success(_unitOfWork.MapTo<ProjectDTO>(result));
    }
    catch (Exception ex)
    {
      // TODO: Log details here
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result> UpdateProjectAsync(Guid projectId, ProjectDTO projectDTO, ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      var aggregateToUpdate = await _repository!.GetByIdAsync(projectId, cts.Token);
      if (aggregateToUpdate == null) return Result.NotFound();

      aggregateToUpdate = ProjectFactory.CreateProject(projectDTO.Name, projectDTO.Priority, (byte)RecordStatus.Approved, serviceHeader, aggregateToUpdate);

      await _repository!.UpdateAsync(aggregateToUpdate, cts.Token);
      await _unitOfWork.SaveAsync();
      var domainEvent = new ProjectUpdatedEvent(aggregateToUpdate);
      await _mediator.Publish(domainEvent, cts.Token).ConfigureAwait(false);
      return Result.Success();
    }
    catch (Exception ex)
    {
      // TODO: Log details here
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result> DeleteProjectAsync(Guid projectId, ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      var aggregateSpec = new ProjectByIdWithItemsSpec(projectId);
      var aggregateToDelete = await _repository!.FirstOrDefaultAsync(aggregateSpec, cts.Token);
      if (aggregateToDelete == null) return Result.NotFound();

      if (aggregateToDelete.Items == null || aggregateToDelete.Items.Any()) return Result.Error(new[] { "Projects' Items found." });

      await _repository!.DeleteAsync(aggregateToDelete, cts.Token);
      var domainEvent = new ProjectDeletedEvent(projectId);
      await _mediator.Publish(domainEvent, cts.Token).ConfigureAwait(false);
      return Result.Success();
    }
    catch (Exception ex)
    {
      // TODO: Log details here
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result<ProjectDTO>> FindProjectAsync(Guid projectId, ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      var aggregateSpec = new ProjectByIdWithItemsSpec(projectId);
      var projectDTO = await _readRepository!.FirstOrDefaultAsync<ProjectDTO>(aggregateSpec, cts.Token);

      if (projectDTO == null)
      {
        return Result.NotFound();
      }

      return new Result<ProjectDTO>(projectDTO);
    }
    catch (Exception ex)
    {
      // TODO: Log details here
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result<List<ProjectDTO>>> FindProjectsAsync(ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      var projectDTOs = await _readRepository!.ListAsync<ProjectDTO>(cts.Token);

      if (projectDTOs == null)
      {
        return Result.NotFound();
      }

      return new Result<List<ProjectDTO>>(projectDTOs);
    }
    catch (Exception ex)
    {
      // TODO: Log details here
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result<PageCollectionInfo<ProjectDTO>>> GetProjectsWithFiltersAndInPageAsync(string? searchString, int pageNumber, int pageSize, string sortColumn, string sortDirection, ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      var allProjectSpec = new ProjectsWithFiltersAndInPageSpec(searchString, sortColumn, sortDirection);

      var projects = await PaginatedIAggregateRootList<Project, ProjectDTO>.CreatePageCollectionInfoAsync(_unitOfWork, allProjectSpec, pageNumber, pageSize, _unitOfWork.MapTo<ProjectDTO>, cts.Token);

      if (projects != null)
      {
        if (projects.PageCollection == null)
        {
          return Result<PageCollectionInfo<ProjectDTO>>.NotFound();
        }

        return new Result<PageCollectionInfo<ProjectDTO>>(projects);
      }

      return Result<PageCollectionInfo<ProjectDTO>>.NotFound("");
    }
    catch (Exception ex)
    {
      // TODO: Log details here
      return Result<PageCollectionInfo<ProjectDTO>>.Error(new[] { ex.Message });
    }
  }

  public async Task<Result> MarkProjectItemsCompleteAsync(Guid projectId, Guid itemId, ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      var projectSpec = new ProjectByIdWithItemsSpec(projectId);
      var project = await _repository!.FirstOrDefaultAsync(projectSpec, cts.Token);
      if (project == null) return Result.NotFound("No such project");

      var toDoItem = project.Items.FirstOrDefault(item => item.Id == itemId);
      if (toDoItem == null) return Result.NotFound("No such item.");

      toDoItem = ToDoItemFactory.CreateToDoItem(toDoItem.Title, toDoItem.Description, toDoItem.ContributorId, (byte)RecordStatus.Approved, serviceHeader, toDoItem);

      toDoItem.MarkComplete();

      project = ProjectFactory.CreateProject(project.Name, project.Priority, (byte)RecordStatus.Approved, serviceHeader, project);

      await _repository!.UpdateAsync(project, cts.Token);
      await _unitOfWork.SaveAsync();
      var domainEvent = new ProjectUpdatedEvent(project);
      await _mediator.Publish(domainEvent, cts.Token).ConfigureAwait(false);
      return Result.Success();
    }
    catch (Exception ex)
    {
      // TODO: Log details here
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result> MarkProjectAllItemsCompleteAsync(Guid projectId, ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      var projectSpec = new ProjectByIdWithItemsSpec(projectId);
      var project = await _repository!.FirstOrDefaultAsync(projectSpec, cts.Token);
      if (project == null) return Result.NotFound("No such project");

      if (project.Items == null || !project.Items.Any()) return Result.NotFound("No items found.");

      project.Items.ToList().ForEach(toDoItem =>
      {
        toDoItem = ToDoItemFactory.CreateToDoItem(toDoItem.Title, toDoItem.Description, toDoItem.ContributorId, (byte)RecordStatus.Approved, serviceHeader, toDoItem);

        toDoItem.MarkComplete();
      });

      project = ProjectFactory.CreateProject(project.Name, project.Priority, (byte)RecordStatus.Approved, serviceHeader, project);

      await _repository!.UpdateAsync(project, cts.Token);
      await _unitOfWork.SaveAsync();
      var domainEvent = new ProjectUpdatedEvent(project);
      await _mediator.Publish(domainEvent, cts.Token).ConfigureAwait(false);
      return Result.Success();
    }
    catch (Exception ex)
    {
      // TODO: Log details here
      return Result.Error(new[] { ex.Message });
    }
  }
}

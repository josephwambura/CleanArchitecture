using Ardalis.Result;

using Clean.Architecture.Application.DTO.ProjectAggregate;
using Clean.Architecture.Application.Interfaces;
using Clean.Architecture.Core.ProjectAggregate;
using Clean.Architecture.Core.ProjectAggregate.Specifications;
using Clean.Architecture.SharedKernel.Interfaces;

namespace Clean.Architecture.Application.Services;

public class ToDoItemService : IToDoItemService
{
  private readonly IRepository<Project> _repository;

  public ToDoItemService(IRepository<Project> repository)
  {
    _repository = repository;
  }

  public async Task<Result<List<ToDoItemDTO>>> GetAllIncompleteItemsAsync(Guid projectId, string searchString)
  {
    if (string.IsNullOrEmpty(searchString))
    {
      var errors = new List<ValidationError>
      {
        new() { Identifier = nameof(searchString), ErrorMessage = $"{nameof(searchString)} is required." }
      };

      return Result<List<ToDoItemDTO>>.Invalid(errors);
    }

    var projectSpec = new ProjectByIdWithItemsSpec(projectId);
    var project = await _repository.FirstOrDefaultAsync(projectSpec);

    // TODO: Optionally use Ardalis.GuardClauses Guard.Against.NotFound and catch
    if (project == null)
    {
      return Result<List<ToDoItemDTO>>.NotFound();
    }

    var incompleteSpec = new IncompleteItemsSearchSpec(searchString);
    try
    {
      var items = incompleteSpec.Evaluate(project.Items).Select(i => ToDoItemDTO.FromToDoItem(i)).ToList();

      return new Result<List<ToDoItemDTO>>(items);
    }
    catch (Exception ex)
    {
      // TODO: Log details here
      return Result<List<ToDoItemDTO>>.Error(new[] { ex.Message });
    }
  }

  public async Task<Result<ToDoItemDTO>> GetNextIncompleteItemAsync(Guid projectId)
  {
    var projectSpec = new ProjectByIdWithItemsSpec(projectId);
    var project = await _repository.FirstOrDefaultAsync(projectSpec);
    if (project == null)
    {
      return Result<ToDoItemDTO>.NotFound();
    }

    var incompleteSpec = new IncompleteItemsSpec();
    var items = incompleteSpec.Evaluate(project.Items).ToList();
    if (items.Count == 0)
    {
      return Result<ToDoItemDTO>.NotFound();
    }

    return new Result<ToDoItemDTO>(ToDoItemDTO.FromToDoItem(items.First()));
  }
}

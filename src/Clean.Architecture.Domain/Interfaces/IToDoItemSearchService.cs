using Ardalis.Result;

using Clean.Architecture.Domain.ProjectAggregate;

namespace Clean.Architecture.Domain.Interfaces;

public interface IToDoItemSearchService
{
  Task<Result<ToDoItem>> GetNextIncompleteItemAsync(Guid projectId);
  Task<Result<List<ToDoItem>>> GetAllIncompleteItemsAsync(Guid projectId, string searchString);
}

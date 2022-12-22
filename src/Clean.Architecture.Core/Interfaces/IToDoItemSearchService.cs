using Ardalis.Result;
using Clean.Architecture.Core.ProjectAggregate;

namespace Clean.Architecture.Core.Interfaces;

public interface IToDoItemSearchService
{
  Task<Result<ToDoItem>> GetNextIncompleteItemAsync(Guid projectId);
  Task<Result<List<ToDoItem>>> GetAllIncompleteItemsAsync(Guid projectId, string searchString);
}

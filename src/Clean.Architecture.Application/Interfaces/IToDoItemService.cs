using Ardalis.Result;

using Clean.Architecture.Application.DTO.ProjectAggregate;

namespace Clean.Architecture.Application.Interfaces;

public interface IToDoItemService
{
  Task<Result<ToDoItemDTO>> GetNextIncompleteItemAsync(Guid projectId);
  Task<Result<List<ToDoItemDTO>>> GetAllIncompleteItemsAsync(Guid projectId, string searchString);
}

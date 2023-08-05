using Ardalis.Specification;

using Clean.Architecture.Domain.ProjectAggregate;

namespace Clean.Architecture.Domain.ProjectAggregate.Specifications;

public class IncompleteItemsSpec : Specification<ToDoItem>
{
  public IncompleteItemsSpec()
  {
    Query.Where(item => !item.IsDone);
  }
}

using Clean.Architecture.Core.ProjectAggregate;

namespace Clean.Architecture.UnitTests;

// Learn more about test builders:
// https://ardalis.com/improve-tests-with-the-builder-pattern-for-test-data
public class ToDoItemBuilder
{
  private ToDoItem _todo = new ToDoItem();

  public ToDoItemBuilder Id(Guid id)
  {
    _todo.Id = id;
    return this;
  }

  public ToDoItemBuilder Title(string title)
  {
    _todo.Title = title;
    return this;
  }

  public ToDoItemBuilder Description(string description)
  {
    _todo.Description = description;
    return this;
  }

  public ToDoItemBuilder WithDefaultValues()
  {
    _todo = new ToDoItem() { Id = Guid.Parse("91C63CEA-7596-4E04-8C5E-880B2B2625A1"), Title = "Test Item", Description = "Test Description" };

    return this;
  }

  public ToDoItem Build() => _todo;
}

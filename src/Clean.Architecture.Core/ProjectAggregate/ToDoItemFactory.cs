using Clean.Architecture.SharedKernel.Extensions;
using Clean.Architecture.SharedKernel.Utils;

namespace Clean.Architecture.Core.ProjectAggregate;

public static class ToDoItemFactory
{
  public static ToDoItem CreateToDoItem(string title, string description, Guid? contributor, byte recordStatus, ServiceHeader serviceHeader, ToDoItem? original = null)
  {
    var entity = original ?? new ToDoItem()
    {
      Title = title,
      Description = description
    };

    if (contributor != null)
      entity.AddContributor(contributor);
    else
      entity.RemoveContributor();

    EntityFactoryExtensions.BaseEntityDetails(entity, serviceHeader, original);

    return entity;
  }
}

using Clean.Architecture.SharedKernel.Extensions;
using Clean.Architecture.SharedKernel.Utils;

namespace Clean.Architecture.Core.ProjectAggregate;

public static class ProjectFactory
{
  public static Project CreateProject(string name, PriorityStatus priorityStatus, byte recordStatus, ServiceHeader serviceHeader, Project? original = null)
  {
    var entity = original ?? new Project(name, priorityStatus);

    EntityFactoryExtensions.BaseEntityDetails(entity, serviceHeader, original);

    return entity;
  }
}

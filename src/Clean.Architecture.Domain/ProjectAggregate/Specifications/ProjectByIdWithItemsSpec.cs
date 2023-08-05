using Ardalis.Specification;

using Clean.Architecture.Domain.ProjectAggregate;

namespace Clean.Architecture.Domain.ProjectAggregate.Specifications;

public class ProjectByIdWithItemsSpec : Specification<Project>, ISingleResultSpecification<Project>
{
  public ProjectByIdWithItemsSpec(Guid projectId)
  {
    Query
        .Where(project => project.Id == projectId)
        .Include(project => project.Items);
  }
}

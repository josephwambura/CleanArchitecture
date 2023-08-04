using Ardalis.Specification;

namespace Clean.Architecture.Core.ProjectAggregate.Specifications;

public class ProjectByIdWithItemsSpec : Specification<Project>, ISingleResultSpecification<Project>
{
  public ProjectByIdWithItemsSpec(Guid projectId)
  {
    Query
        .Where(project => project.Id == projectId)
        .Include(project => project.Items);
  }
}

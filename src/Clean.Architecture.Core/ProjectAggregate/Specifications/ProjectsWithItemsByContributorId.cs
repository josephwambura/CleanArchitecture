using Ardalis.Specification;

namespace Clean.Architecture.Core.ProjectAggregate.Specifications;

public class ProjectsWithItemsByContributorIdSpec : Specification<Project>, ISingleResultSpecification<Project>
{
  public ProjectsWithItemsByContributorIdSpec(Guid contributorId)
  {
    Query
        .Where(project => project.Items.Any(item => item.ContributorId == contributorId))
        .Include(project => project.Items);
  }
}

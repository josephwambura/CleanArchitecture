using Ardalis.Specification;

using Clean.Architecture.SharedKernel.Extensions;

namespace Clean.Architecture.Core.ProjectAggregate.Specifications;

public class ProjectsWithFiltersAndInPageSpec : Specification<Project>, ISingleResultSpecification<Project>
{
  public ProjectsWithFiltersAndInPageSpec(string? searchString, string sortColumn, string sortDirection)
  {
    if (!string.IsNullOrWhiteSpace(searchString))
    {
      Query
        .Where(project => project.Name.Contains(searchString));
    }

    Query.ApplyOrdering(sortColumn, sortDirection);

    //Query
    //    .EnableCache(nameof(ProjectsWithFiltersAndInPageSpec));
  }
}

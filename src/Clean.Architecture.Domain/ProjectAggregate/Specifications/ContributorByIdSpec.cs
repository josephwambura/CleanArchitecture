using Ardalis.Specification;

using Clean.Architecture.Domain.ContributorAggregate;

namespace Clean.Architecture.Domain.ProjectAggregate.Specifications;

public class ContributorByIdSpec : Specification<Contributor>, ISingleResultSpecification<Contributor>
{
  public ContributorByIdSpec(Guid contributorId)
  {
    Query
        .Where(contributor => contributor.Id == contributorId);
  }
}

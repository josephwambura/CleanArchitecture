﻿using Ardalis.Specification;

using Clean.Architecture.Core.ContributorAggregate;

namespace Clean.Architecture.Core.ProjectAggregate.Specifications;

public class ContributorByIdSpec : Specification<Contributor>, ISingleResultSpecification<Contributor>
{
  public ContributorByIdSpec(Guid contributorId)
  {
    Query
        .Where(contributor => contributor.Id == contributorId);
  }
}
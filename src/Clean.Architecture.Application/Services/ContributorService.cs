using Ardalis.Result;

using Clean.Architecture.Application.Interfaces;
using Clean.Architecture.Domain.ContributorAggregate;
using Clean.Architecture.Domain.ContributorAggregate.Events;
using Clean.Architecture.SharedKernel.Interfaces;

using MediatR;

namespace Clean.Architecture.Application.Services;

public class ContributorService : IContributorService
{
  private readonly IRepository<Contributor> _repository;
  private readonly IMediator _mediator;

  public ContributorService(IRepository<Contributor> repository, IMediator mediator)
  {
    _repository = repository;
    _mediator = mediator;
  }

  public async Task<Result> DeleteContributor(Guid contributorId)
  {
    var aggregateToDelete = await _repository.GetByIdAsync(contributorId);
    if (aggregateToDelete == null) return Result.NotFound();

    await _repository.DeleteAsync(aggregateToDelete);
    var domainEvent = new ContributorDeletedEvent(contributorId);
    await _mediator.Publish(domainEvent).ConfigureAwait(false);
    return Result.Success();
  }
}

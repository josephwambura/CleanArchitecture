using Clean.Architecture.Core.UserManagementModule.ApplicationUserAggregate;

using MediatR;

namespace Clean.Architecture.Infrastructure.Data.Auth;

public class AuthDomainEventDispatcher : IAuthDomainEventDispatcher
{
  private readonly IMediator _mediator;

  public AuthDomainEventDispatcher(IMediator mediator)
  {
    _mediator = mediator;
  }

  public async Task DispatchAndClearEvents(IEnumerable<ApplicationUser> entitiesWithEvents)
  {
    foreach (var entity in entitiesWithEvents)
    {
      var events = entity.DomainEvents.ToArray();
      entity.ClearDomainEvents();
      foreach (var domainEvent in events)
      {
        await _mediator.Publish(domainEvent).ConfigureAwait(ConfigureAwaitOptions.None);
      }
    }
  }
}

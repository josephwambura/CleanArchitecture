using Clean.Architecture.Core.UserManagementModule.ApplicationUserAggregate;

namespace Clean.Architecture.Infrastructure.Data.Auth;

public interface IAuthDomainEventDispatcher
{
  Task DispatchAndClearEvents(IEnumerable<ApplicationUser> entitiesWithEvents);
}

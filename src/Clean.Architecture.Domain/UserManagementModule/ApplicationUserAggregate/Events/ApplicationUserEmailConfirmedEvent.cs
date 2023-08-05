using Clean.Architecture.Domain.UserManagementModule.ApplicationUserAggregate;
using Clean.Architecture.SharedKernel;

namespace Clean.Architecture.Domain.UserManagementModule.ApplicationUserAggregate.Events;

public class ApplicationUserEmailConfirmedEvent : DomainEventBase
{
  public ApplicationUser ApplicationUser { get; set; }

  public ApplicationUserEmailConfirmedEvent(ApplicationUser applicationUser)
  {
    ApplicationUser = applicationUser;
  }
}

using Clean.Architecture.Domain.UserManagementModule.ApplicationUserAggregate;
using Clean.Architecture.SharedKernel;

namespace Clean.Architecture.Domain.UserManagementModule.ApplicationUserAggregate.Events;

public class ApplicationUserLoggedInEvent : DomainEventBase
{
  public ApplicationUser ApplicationUser { get; set; }

  public ApplicationUserLoggedInEvent(ApplicationUser applicationUser)
  {
    ApplicationUser = applicationUser;
  }
}

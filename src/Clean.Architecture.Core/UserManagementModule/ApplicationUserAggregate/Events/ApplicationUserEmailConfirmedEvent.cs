using Clean.Architecture.SharedKernel;

namespace Clean.Architecture.Core.UserManagementModule.ApplicationUserAggregate.Events;

public class ApplicationUserEmailConfirmedEvent : DomainEventBase
{
  public ApplicationUser ApplicationUser { get; set; }

  public ApplicationUserEmailConfirmedEvent(ApplicationUser applicationUser)
  {
    ApplicationUser = applicationUser;
  }
}

using Clean.Architecture.SharedKernel;

namespace Clean.Architecture.Core.UserManagementModule.ApplicationUserAggregate.Events;

public class ApplicationUserUpdatedEvent : DomainEventBase
{
  public ApplicationUser ApplicationUser { get; set; }

  public ApplicationUserUpdatedEvent(ApplicationUser applicationUser)
  {
    ApplicationUser = applicationUser;
  }
}

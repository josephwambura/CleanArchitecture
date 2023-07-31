using Clean.Architecture.SharedKernel;

namespace Clean.Architecture.Core.UserManagementModule.ApplicationUserAggregate.Events;

public class ApplicationUserDeletedEvent : DomainEventBase
{
  public Guid ApplicationUserId { get; set; }

  public ApplicationUserDeletedEvent(Guid applicationUserId)
  {
    ApplicationUserId = applicationUserId;
  }
}

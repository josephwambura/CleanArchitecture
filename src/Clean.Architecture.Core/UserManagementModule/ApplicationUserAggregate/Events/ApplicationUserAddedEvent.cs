﻿using Clean.Architecture.SharedKernel;

namespace Clean.Architecture.Core.UserManagementModule.ApplicationUserAggregate.Events;

public class ApplicationUserAddedEvent : DomainEventBase
{
  public ApplicationUser ApplicationUser { get; set; }
  public string Password { get; set; }

  public ApplicationUserAddedEvent(ApplicationUser applicationUser, string password)
  {
    ApplicationUser = applicationUser;
    Password = password;
  }
}
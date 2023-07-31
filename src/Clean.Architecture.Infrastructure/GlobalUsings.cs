global using Ardalis.Result;

global using Clean.Architecture.Core.MessagingModule.EmailAlertAggregate;
global using Clean.Architecture.Core.MessagingModule.TextAlertAggregate;
global using Clean.Architecture.Core.MessagingModule.FCMAlertAggregate;
global using Clean.Architecture.Core.MessagingModule.NotificationRecipientAggregate;
global using Clean.Architecture.SharedKernel.Utils;

global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;

#region Infrastructure
global using Clean.Architecture.Infrastructure.Data;
global using Clean.Architecture.Infrastructure.Interfaces;
#endregion
#region SharedKernel
global using Clean.Architecture.SharedKernel;
global using Clean.Architecture.SharedKernel.Interfaces;
#endregion

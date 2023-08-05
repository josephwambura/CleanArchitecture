using Clean.Architecture.Domain.ValueObjects;
using Clean.Architecture.SharedKernel.Utils;

namespace Clean.Architecture.Domain.UserManagementModule.ApplicationUserAggregate;

public static class ApplicationUserFactory
{
  public static ApplicationUser CreateApplicationUser(string name, string email, bool emailConfirmed, string phoneNumber, bool phoneNumberConfirmed, byte recordStatus, ServiceHeader serviceHeader, ApplicationUser? original = null)
  {
    var entity = original ?? new ApplicationUser();

    entity.UserName = name;
    entity.NormalizedUserName = entity.UserName?.ToUpper();
    entity.Email = email;
    entity.NormalizedEmail = entity.Email?.ToUpper();
    entity.EmailConfirmed = emailConfirmed;
    entity.PhoneNumber = phoneNumber;
    entity.PhoneNumberConfirmed = phoneNumberConfirmed;

    entity.TransactionEnvironment = new TransactionEnvironment
    {
      EnvironmentIPAddress = original?.TransactionEnvironment?.EnvironmentIPAddress ?? serviceHeader.EnvironmentIPAddress,
      EnvironmentMACAddress = original?.TransactionEnvironment?.EnvironmentMACAddress ?? serviceHeader.EnvironmentMACAddress,
      EnvironmentMotherboardSerialNumber = original?.TransactionEnvironment?.EnvironmentMotherboardSerialNumber ?? serviceHeader.EnvironmentMotherboardSerialNumber,
      EnvironmentProcessorId = original?.TransactionEnvironment?.EnvironmentProcessorId ?? serviceHeader.EnvironmentProcessorId,
      EnvironmentUserName = original?.TransactionEnvironment?.EnvironmentUserName ?? serviceHeader.EnvironmentUserName,
      EnvironmentMachineName = original?.TransactionEnvironment?.EnvironmentMachineName ?? serviceHeader.EnvironmentMachineName,
      EnvironmentDomainName = original?.TransactionEnvironment?.EnvironmentDomainName ?? serviceHeader.EnvironmentDomainName,
      EnvironmentOSVersion = original?.TransactionEnvironment?.EnvironmentOSVersion ?? serviceHeader.EnvironmentOSVersion,

      ThirdPartyClientIPAddress = original?.TransactionEnvironment?.ClientIPAddress ?? serviceHeader.ClientIPAddress,

      ClientIPAddress = original?.TransactionEnvironment?.ClientIPAddress ?? serviceHeader.ClientIPAddress,
      ClientUserAgent = original?.TransactionEnvironment?.ClientUserAgent ?? serviceHeader.ClientUserAgent,
      ClientBrowser = original?.TransactionEnvironment?.ClientBrowser ?? serviceHeader.ClientBrowser?.ToString(),
      ClientLocation = original?.TransactionEnvironment?.ClientLocation ?? serviceHeader.ClientLocation?.ToString(),
    };

    if (original == null)
    {
      entity.CreatedBy = string.Format("{0}", serviceHeader.ApplicationUserName).Length == 0 ? "___SYS___" : serviceHeader.ApplicationUserName!;
      entity.CreatedDate = DateTime.UtcNow;
    }
    else
    {
      entity.ModifiedBy = string.Format("{0}", serviceHeader.ApplicationUserName).Length == 0 ? "___SYS___" : serviceHeader.ApplicationUserName;
      entity.ModifiedDate = DateTime.UtcNow;
    }

    entity.Id = entity.IsTransient() ? IdentityGenerator.NewSequentialGuid().ToString() : entity.Id;

    entity.RecordStatus = (byte)RecordStatus.Approved;

    return entity;
  }
}

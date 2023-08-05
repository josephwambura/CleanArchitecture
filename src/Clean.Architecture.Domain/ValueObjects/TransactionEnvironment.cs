using Clean.Architecture.SharedKernel;

namespace Clean.Architecture.Domain.ValueObjects;

public class TransactionEnvironment : ValueObject
{
  public string? EnvironmentUserName { get; set; }
  public string? EnvironmentMachineName { get; set; }
  public string? EnvironmentDomainName { get; set; }
  public string? EnvironmentOSVersion { get; set; }
  public string? EnvironmentMACAddress { get; set; }
  public string? EnvironmentMotherboardSerialNumber { get; set; }
  public string? EnvironmentProcessorId { get; set; }
  public string? EnvironmentIPAddress { get; set; }
  public string? ThirdPartyClientIPAddress { get; set; }
  public string? ClientIPAddress { get; set; }
  public string? ClientBrowser { get; set; }
  public string? ClientUserAgent { get; set; }
  public string? ClientLocation { get; set; }

  public TransactionEnvironment() { }

  public TransactionEnvironment(string userName, string machineName, string domainName, string oSVersion, string mACAddress, string motherboardSerialNumber, string processorId, string iPAddress, string thirdPartyClientIPAddress, string clientIPAddress, string clientBrowser, string clientUserAgent, string clientLocation)
  {
    EnvironmentUserName = userName;
    EnvironmentMachineName = machineName;
    EnvironmentDomainName = domainName;
    EnvironmentOSVersion = oSVersion;
    EnvironmentMACAddress = mACAddress;
    EnvironmentMotherboardSerialNumber = motherboardSerialNumber;
    EnvironmentProcessorId = processorId;
    EnvironmentIPAddress = iPAddress;
    ThirdPartyClientIPAddress = thirdPartyClientIPAddress;
    ClientIPAddress = clientIPAddress;
    ClientBrowser = clientBrowser;
    ClientUserAgent = clientUserAgent;
    ClientLocation = clientLocation;
  }

  protected override IEnumerable<object> GetEqualityComponents()
  {
    // Using a yield return statement to return each element one at a time

    yield return EnvironmentUserName ?? string.Empty;
    yield return EnvironmentMachineName ?? string.Empty;
    yield return EnvironmentDomainName ?? string.Empty;
    yield return EnvironmentOSVersion ?? string.Empty;
    yield return EnvironmentMACAddress ?? string.Empty;
    yield return EnvironmentMotherboardSerialNumber ?? string.Empty;
    yield return EnvironmentProcessorId ?? string.Empty;
    yield return EnvironmentIPAddress ?? string.Empty;
    yield return ThirdPartyClientIPAddress ?? string.Empty;
    yield return ClientIPAddress ?? string.Empty;
    yield return ClientBrowser ?? string.Empty;
    yield return ClientUserAgent ?? string.Empty;
    yield return ClientLocation ?? string.Empty;
  }

  public override string ToString()
  {
    return $"TransactionEnvironment: {EnvironmentMachineName}, {EnvironmentProcessorId}, {ClientIPAddress}";
  }
}

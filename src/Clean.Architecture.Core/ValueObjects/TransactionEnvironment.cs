using Clean.Architecture.SharedKernel;

namespace Clean.Architecture.Core.ValueObjects;

public class TransactionEnvironment : ValueObject
{
  public String? EnvironmentUserName { get; set; }
  public String? EnvironmentMachineName { get; set; }
  public String? EnvironmentDomainName { get; set; }
  public String? EnvironmentOSVersion { get; set; }
  public String? EnvironmentMACAddress { get; set; }
  public String? EnvironmentMotherboardSerialNumber { get; set; }
  public String? EnvironmentProcessorId { get; set; }
  public String? EnvironmentIPAddress { get; set; }
  public String? ThirdPartyClientIPAddress { get; set; }
  public String? ClientIPAddress { get; set; }
  public String? ClientBrowser { get; set; }
  public String? ClientUserAgent { get; set; }
  public String? ClientLocation { get; set; }

  public TransactionEnvironment() { }

  public TransactionEnvironment(string userName, string machineName, string domainName, string oSVersion, string mACAddress, string motherboardSerialNumber, string processorId, string iPAddress, string thirdPartyClientIPAddress, string clientIPAddress, string clientBrowser, string clientUserAgent, string clientLocation)
  {
    this.EnvironmentUserName = userName;
    this.EnvironmentMachineName = machineName;
    this.EnvironmentDomainName = domainName;
    this.EnvironmentOSVersion = oSVersion;
    this.EnvironmentMACAddress = mACAddress;
    this.EnvironmentMotherboardSerialNumber = motherboardSerialNumber;
    this.EnvironmentProcessorId = processorId;
    this.EnvironmentIPAddress = iPAddress;
    this.ThirdPartyClientIPAddress = thirdPartyClientIPAddress;
    this.ClientIPAddress = clientIPAddress;
    this.ClientBrowser = clientBrowser;
    this.ClientUserAgent = clientUserAgent;
    this.ClientLocation = clientLocation;
  }

  protected override IEnumerable<object> GetEqualityComponents()
  {
    // Using a yield return statement to return each element one at a time

    yield return EnvironmentUserName ?? String.Empty;
    yield return EnvironmentMachineName ?? String.Empty;
    yield return EnvironmentDomainName ?? String.Empty;
    yield return EnvironmentOSVersion ?? String.Empty;
    yield return EnvironmentMACAddress ?? String.Empty;
    yield return EnvironmentMotherboardSerialNumber ?? String.Empty;
    yield return EnvironmentProcessorId ?? String.Empty;
    yield return EnvironmentIPAddress ?? String.Empty;
    yield return ThirdPartyClientIPAddress ?? String.Empty;
    yield return ClientIPAddress ?? String.Empty;
    yield return ClientBrowser ?? String.Empty;
    yield return ClientUserAgent ?? String.Empty;
    yield return ClientLocation ?? String.Empty;
  }

  public override string ToString()
  {
    return $"TransactionEnvironment: {EnvironmentMachineName}, {EnvironmentProcessorId}, {ClientIPAddress}";
  }
}

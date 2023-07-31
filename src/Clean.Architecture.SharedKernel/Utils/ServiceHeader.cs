namespace Clean.Architecture.SharedKernel.Utils;

public class ServiceHeader
{
  public string? ApplicationDomainName { get; set; }
  public string? ApplicationUserName { get; set; }
  public string? EnvironmentUserName { get; set; }
  public string? EnvironmentMachineName { get; set; }
  public string? EnvironmentDomainName { get; set; }
  public string? EnvironmentOSVersion { get; set; }
  public string? EnvironmentMACAddress { get; set; }
  public string? EnvironmentMotherboardSerialNumber { get; set; }
  public string? EnvironmentProcessorId { get; set; }
  public string? EnvironmentIPAddress { get; set; }
  public string? ClientIPAddress { get; set; }
  public string? ClientSessionID { get; set; }
  public string? ClientUserHostAddress { get; set; }
  public string? ClientUserHostName { get; set; }
  public string? ClientUserAgent { get; set; }
  public BrowserInfo? ClientBrowser { get; set; }
  public LocationInfo? ClientLocation { get; set; }
}

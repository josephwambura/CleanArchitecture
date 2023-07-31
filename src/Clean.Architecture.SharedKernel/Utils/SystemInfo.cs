using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Clean.Architecture.SharedKernel.Utils;

// API is supported on Windows, iOS from version 14.0, and MacCatalyst from version 14.0.
//[SupportedOSPlatform("windows")]
// An API supported only on Linux.
//[SupportedOSPlatform("linux")]
//[SupportedOSPlatform("ios14.0")] // MacCatalyst is a superset of iOS, therefore it's also supported.
public class SystemInfo
{
  public string? IPAddress { get; set; }
  public string? MACAddress { get; set; }
  public string? MotherboardSerialNumber { get; set; }
  public string? ProcessorId { get; set; }
  public string UserName { get; set; }
  public string MachineName { get; set; }
  public string DomainName { get; set; }

  public SystemInfo()
  {
    UserName = Environment.UserName;
    MachineName = Environment.MachineName;
    DomainName = Environment.UserDomainName;
    ProcessorId = GetProcessorID();
    MotherboardSerialNumber = GetSerialNumber();
    MACAddress = GetMacAddress();
    IPAddress = GetIpAddress();
  }

  static string? GetProcessorID()
  {
    string? sProcessorID = null;

    if (OperatingSystem.IsWindows() && RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
    {
      Debug.Assert(OperatingSystem.IsWindows());

      string sQuery = "SELECT ProcessorId FROM Win32_Processor";

      using (System.Management.ManagementObjectSearcher oManagementObjectSearcher = new System.Management.ManagementObjectSearcher("root\\CIMV2", sQuery))
      {
        using (System.Management.ManagementObjectCollection oCollection = oManagementObjectSearcher.Get())
        {
          foreach (System.Management.ManagementObject oManagementObject in oCollection)
          {
            sProcessorID = (string)oManagementObject[nameof(ProcessorId)];

            if (!string.IsNullOrWhiteSpace(sProcessorID))
              break;
          }
        }
      }
    }

    return (sProcessorID);
  }

  static string? GetSerialNumber()
  {
    string? sSerialNumber = null;

    if (OperatingSystem.IsWindows() && RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
    {
      Debug.Assert(OperatingSystem.IsWindows());

      string sQuery = "SELECT SerialNumber FROM Win32_BaseBoard";

      using (System.Management.ManagementObjectSearcher oManagementObjectSearcher = new System.Management.ManagementObjectSearcher("root\\CIMV2", sQuery))
      {
        using (System.Management.ManagementObjectCollection oCollection = oManagementObjectSearcher.Get())
        {
          foreach (System.Management.ManagementObject oManagementObject in oCollection)
          {
            sSerialNumber = (string)oManagementObject["SerialNumber"];

            if (!string.IsNullOrWhiteSpace(sSerialNumber))
              break;
          }
        }
      }
    }

    return (sSerialNumber);
  }

  static string? GetMacAddress()
  {
    string? sMACAddress = null;

    if (OperatingSystem.IsWindows() && RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
    {
      Debug.Assert(OperatingSystem.IsWindows());

      string sQuery = "SELECT MACAddress FROM Win32_NetworkAdapterConfiguration where IPEnabled=true";

      using (System.Management.ManagementObjectSearcher oManagementObjectSearcher = new System.Management.ManagementObjectSearcher("root\\CIMV2", sQuery))
      {
        using (System.Management.ManagementObjectCollection oCollection = oManagementObjectSearcher.Get())
        {
          foreach (System.Management.ManagementObject oManagementObject in oCollection)
          {
            sMACAddress = (string)oManagementObject[nameof(MACAddress)];

            if (!string.IsNullOrWhiteSpace(sMACAddress))
              break;
          }
        }
      }
    }

    return (sMACAddress);
  }

  static string? GetIpAddress()
  {
    string? sIPAddress = null;

    if (OperatingSystem.IsWindows() && RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
    {
      Debug.Assert(OperatingSystem.IsWindows());

      string sQuery = "SELECT IPAddress FROM Win32_NetworkAdapterConfiguration where IPEnabled=true";

      using (System.Management.ManagementObjectSearcher oManagementObjectSearcher = new System.Management.ManagementObjectSearcher("root\\CIMV2", sQuery))
      {
        using (System.Management.ManagementObjectCollection oCollection = oManagementObjectSearcher.Get())
        {
          foreach (System.Management.ManagementObject oManagementObject in oCollection)
          {
            if (oManagementObject[nameof(IPAddress)] is Array)
            {
              sIPAddress = string.Join(",", (string[])oManagementObject[nameof(IPAddress)]);
            }
            else sIPAddress = (string)oManagementObject[nameof(IPAddress)];

            if (!string.IsNullOrWhiteSpace(sIPAddress))
              break;
          }
        }
      }
    }

    return (sIPAddress);
  }
}

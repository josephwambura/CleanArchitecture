using Clean.Architecture.SharedKernel.Utils;

using Xunit;

namespace Clean.Architecture.UnitTests.SharedKernel.Utils;

public class UtilsFeaturesConstructor
{
  [Fact]
  public void GetMachineDetails_SystemInfo()
  {
    var systemInfo = new SystemInfo();

    Assert.NotNull(systemInfo);
    Assert.NotEmpty(systemInfo.UserName);
    Assert.NotEmpty(systemInfo.MachineName);
    //Assert.NotNull(systemInfo.IPAddress);
    //Assert.NotEmpty(systemInfo.IPAddress);
    Assert.NotNull(systemInfo.ProcessorId);
    Assert.NotEmpty(systemInfo.ProcessorId);
    Assert.NotNull(systemInfo.MotherboardSerialNumber);
    Assert.NotEmpty(systemInfo.MotherboardSerialNumber);
  }
}

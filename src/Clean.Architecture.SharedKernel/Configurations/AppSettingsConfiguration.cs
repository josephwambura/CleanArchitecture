namespace Clean.Architecture.SharedKernel.Configurations;

public class AppSettingsConfiguration
{
  public byte RunningEnvironment { get; set; }
  public string? WebApplicationAddress { get; set; }
}

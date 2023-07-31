namespace Clean.Architecture.SharedKernel.Configurations;

public class RedisConfiguration
{
  public string? Host { get; set; }
  public string? Password { get; set; }
  public bool AbortConnect { get; set; }
  public int Port { get; set; }
}

namespace Clean.Architecture.SharedKernel.Extensions;

public static class DateTimeExtensions
{
  public static string ISO8601DateTimeFormat(this DateTimeOffset dateTime) => dateTime.ToUniversalTime().ToString("o");
}

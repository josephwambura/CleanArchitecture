using System.Text.Json;

namespace Clean.Architecture.SharedKernel.Extensions;

public static class JsonSerializerExtensionMethods
{
  public static string ToJson<T>(this T @object, JsonSerializerOptions? jsonSerializerOptions = null)
  {
    return JsonSerializer.Serialize(@object, jsonSerializerOptions ?? new JsonSerializerOptions
    {
      PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    });
  }

  public static T? FromJson<T>(this string text, JsonSerializerOptions? jsonSerializerOptions = null)
  {
    return JsonSerializer.Deserialize<T>(text, jsonSerializerOptions ?? new JsonSerializerOptions
    {
      PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    });
  }
}

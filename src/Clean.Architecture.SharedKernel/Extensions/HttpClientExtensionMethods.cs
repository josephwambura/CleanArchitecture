using Microsoft.Extensions.Logging;

namespace Clean.Architecture.SharedKernel.Extensions;

public static class HttpClientExtensionMethods
{
  public static async Task<T?> GetAndDeSerializeAsync<T>(this HttpClient client, string requestUri, ILogger? logger = null)
  {
    HttpResponseMessage obj = await client.GetAsync(requestUri);
    obj.EnsureSuccessStatusCode();
    string text = await obj.Content.ReadAsStringAsync();
#if DEBUG
    logger?.LogDebug($"Response: {text}");
#endif
    return text.FromJson<T>();
  }

  public static async Task<T?> PostAndDeSerializeAsync<T>(this HttpClient client, string requestUri, HttpContent content, ILogger? logger = null)
  {
    HttpResponseMessage obj = await client.PostAsync(requestUri, content);
    obj.EnsureSuccessStatusCode();
    string text = await obj.Content.ReadAsStringAsync();
#if DEBUG
    logger?.LogDebug($"Response: {text}");
#endif
    return text.FromJson<T>();
  }

  public static async Task<T?> PatchAndDeSerializeAsync<T>(this HttpClient client, string requestUri, HttpContent content, ILogger? logger = null)
  {
    HttpResponseMessage obj = await client.PatchAsync(requestUri, content);
    obj.EnsureSuccessStatusCode();
    string text = await obj.Content.ReadAsStringAsync();
#if DEBUG
    logger?.LogDebug($"Response: {text}");
#endif
    return text.FromJson<T>();
  }
}

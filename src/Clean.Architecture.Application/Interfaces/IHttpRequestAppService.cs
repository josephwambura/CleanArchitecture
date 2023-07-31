using System.Net;

using Clean.Architecture.SharedKernel.Utils;

namespace Clean.Architecture.Application.Interfaces;

public interface IHttpRequestAppService
{
  Task<Tuple<HttpStatusCode, string>> PostAsync(LimitedPool<HttpClient> httpClientPool, string payload, string url, string username, string password);

  Task<Tuple<HttpStatusCode, string>> GetAsync(LimitedPool<HttpClient> httpClientPool, string thirdPartyTransactionId, string url, string username, string password);

  Task<Tuple<HttpStatusCode, string>> AuthAsync(LimitedPool<HttpClient> httpClientPool, string consumerKey, string consumerSecret, string baseUrl);

  Task<Tuple<string, HttpStatusCode>> PostAsync(LimitedPool<HttpClient> httpClientPool, string payload, string url, string bearerToken);

  Task<Tuple<string, HttpStatusCode>> GetAsync(LimitedPool<HttpClient> httpClientPool, string thirdPartyTransactionId, string url, string bearerToken);

  Task<Tuple<string, HttpStatusCode>> DeleteAsync(LimitedPool<HttpClient> httpClientPool, string thirdPartyTransactionId, string url, string bearerToken);

  Task<Tuple<string, HttpStatusCode>> PostAsync(LimitedPool<HttpClient> httpClientPool, string payload, string url);

  Task<Tuple<string, HttpStatusCode>> PostAsync(LimitedPool<HttpClient> httpClientPool, IEnumerable<KeyValuePair<string, string>> values, string url);

  Task<Tuple<HttpStatusCode, string>> PostAsync(LimitedPool<HttpClient> httpClientPool, string payload, string url, string username, string password, string token);
}

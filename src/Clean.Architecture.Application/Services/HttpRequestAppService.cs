using System.Net;
using System.Text;
using Clean.Architecture.Application.Interfaces;
using Clean.Architecture.SharedKernel.Utils;

namespace Clean.Architecture.Application.Services;

public class HttpRequestAppService : IHttpRequestAppService
{
  public async Task<Tuple<HttpStatusCode, string>> PostAsync(LimitedPool<HttpClient> httpClientPool, string payload, string url, string username, string password)
  {
    using (var httpClientContainer = httpClientPool.Get())
    {
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

      var httpContent = new StringContent(payload, Encoding.UTF8, "application/json");

      httpClientContainer.Value.DefaultRequestHeaders.Clear();

      httpClientContainer.Value.DefaultRequestHeaders.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(username + ":" + password)));

      var response = await httpClientContainer.Value.PostAsync(url, httpContent);

      var content = await response.Content.ReadAsStringAsync();

      return new Tuple<HttpStatusCode, string>(response.StatusCode, content);
    }
  }

  public async Task<Tuple<HttpStatusCode, string>> GetAsync(LimitedPool<HttpClient> httpClientPool, string thirdPartyTransactionId, string url, string username, string password)
  {
    using (var httpClientContainer = httpClientPool.Get())
    {
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

      httpClientContainer.Value.DefaultRequestHeaders.Clear();

      httpClientContainer.Value.DefaultRequestHeaders.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(username + ":" + password)));

      var response = await httpClientContainer.Value.GetAsync($"{url}{thirdPartyTransactionId}");

      string content = await response.Content.ReadAsStringAsync();

      return new Tuple<HttpStatusCode, string>(response.StatusCode, content);
    }
  }

  public async Task<Tuple<HttpStatusCode, string>> AuthAsync(LimitedPool<HttpClient> httpClientPool, string consumerKey, string consumerSecret, string baseUrl)
  {
    using (var httpClientContainer = httpClientPool.Get())
    {
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

      httpClientContainer.Value.DefaultRequestHeaders.Clear();

      httpClientContainer.Value.DefaultRequestHeaders.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(consumerKey + ":" + consumerSecret)));

      var response = await httpClientContainer.Value.GetAsync(baseUrl);

      response.EnsureSuccessStatusCode();

      // { StatusCode: 400, ReasonPhrase: 'Bad Request: Invalid Credentials' }

      var content = await response.Content.ReadAsStringAsync();

      return new Tuple<HttpStatusCode, string>(response.StatusCode, content);
    }
  }

  public async Task<Tuple<string, HttpStatusCode>> PostAsync(LimitedPool<HttpClient> httpClientPool, string payload, string url, string bearerToken)
  {
    using (var httpClientContainer = httpClientPool.Get())
    {
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

      ServicePointManager.ServerCertificateValidationCallback += (se, cert, chain, sslError) => true;

      HttpContent httpContent = new StringContent(payload, Encoding.UTF8, "application/json");

      httpClientContainer.Value.DefaultRequestHeaders.Clear();

      httpClientContainer.Value.DefaultRequestHeaders.Add("Authorization", "Bearer " + bearerToken);

      HttpResponseMessage response = await httpClientContainer.Value.PostAsync(url, httpContent);

      var responseMessage = response.Content != null ? await response.Content.ReadAsStringAsync() : string.Empty;

      return new Tuple<string, HttpStatusCode>(responseMessage, response.StatusCode);
    }
  }

  public async Task<Tuple<string, HttpStatusCode>> GetAsync(LimitedPool<HttpClient> httpClientPool, string thirdPartyTransactionId, string url, string bearerToken)
  {
    using (var httpClientContainer = httpClientPool.Get())
    {
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

      ServicePointManager.ServerCertificateValidationCallback += (se, cert, chain, sslError) => true;

      httpClientContainer.Value.DefaultRequestHeaders.Clear();

      httpClientContainer.Value.DefaultRequestHeaders.Add("Authorization", "Bearer " + bearerToken);

      var response = await httpClientContainer.Value.GetAsync($"{url}{thirdPartyTransactionId}");

      string content = await response.Content.ReadAsStringAsync();

      return new Tuple<string, HttpStatusCode>(content, response.StatusCode);
    }
  }

  public async Task<Tuple<string, HttpStatusCode>> DeleteAsync(LimitedPool<HttpClient> httpClientPool, string thirdPartyTransactionId, string url, string bearerToken)
  {
    using (var httpClientContainer = httpClientPool.Get())
    {
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

      httpClientContainer.Value.DefaultRequestHeaders.Clear();

      httpClientContainer.Value.DefaultRequestHeaders.Add("Authorization", "Bearer " + bearerToken);

      var response = await httpClientContainer.Value.DeleteAsync($"{url}{thirdPartyTransactionId}");

      string content = await response.Content.ReadAsStringAsync();

      return new Tuple<string, HttpStatusCode>(content, response.StatusCode);
    }
  }

  public async Task<Tuple<string, HttpStatusCode>> PostAsync(LimitedPool<HttpClient> httpClientPool, string payload, string url)
  {
    using (var httpClientContainer = httpClientPool.Get())
    {
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

      HttpContent httpContent = new StringContent(payload, Encoding.UTF8, "application/json");

      httpClientContainer.Value.DefaultRequestHeaders.Clear();

      HttpResponseMessage response = await httpClientContainer.Value.PostAsync(url, httpContent);

      var responseMessage = response.Content != null ? await response.Content.ReadAsStringAsync() : string.Empty;

      return new Tuple<string, HttpStatusCode>(responseMessage, response.StatusCode);
    }
  }

  public async Task<Tuple<string, HttpStatusCode>> PostAsync(LimitedPool<HttpClient> httpClientPool, IEnumerable<KeyValuePair<string, string>> values, string url)
  {
    using (var httpClientContainer = httpClientPool.Get())
    {
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

      var httpContent = new FormUrlEncodedContent(values);

      httpClientContainer.Value.DefaultRequestHeaders.Clear();

      HttpResponseMessage response = await httpClientContainer.Value.PostAsync(url, httpContent);

      var responseMessage = response.Content != null ? await response.Content.ReadAsStringAsync() : string.Empty;

      return new Tuple<string, HttpStatusCode>(responseMessage, response.StatusCode);
    }
  }

  public async Task<Tuple<HttpStatusCode, string>> PostAsync(LimitedPool<HttpClient> httpClientPool, string payload, string url, string username, string password, string token)
  {
    using (var httpClientContainer = httpClientPool.Get())
    {
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

      var httpContent = new StringContent(payload, Encoding.UTF8, "application/json");

      httpClientContainer.Value.DefaultRequestHeaders.Clear();

      httpClientContainer.Value.DefaultRequestHeaders.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(token + ":" + password)));

      var response = await httpClientContainer.Value.PostAsync(url, httpContent);

      var content = await response.Content.ReadAsStringAsync();

      return new Tuple<HttpStatusCode, string>(response.StatusCode, content);
    }
  }
}

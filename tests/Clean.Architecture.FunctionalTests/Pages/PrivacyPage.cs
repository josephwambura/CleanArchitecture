using Clean.Architecture.Web;
using Xunit;

namespace Clean.Architecture.FunctionalTests.Pages;

[Collection("Sequential")]
public class PrivacyPage : IClassFixture<CustomWebApplicationFactory<Program>>
{
  private readonly HttpClient _client;

  public PrivacyPage(CustomWebApplicationFactory<Program> factory)
  {
    _client = factory.CreateClient();
  }

  [Fact]
  public async Task ReturnsViewWithCorrectMessage()
  {
    HttpResponseMessage response = await _client.GetAsync("/Privacy");
    response.EnsureSuccessStatusCode();
    string stringResponse = await response.Content.ReadAsStringAsync();

    Assert.Contains("Clean.Architecture.Web", stringResponse);
  }
}

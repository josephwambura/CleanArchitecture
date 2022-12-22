using System.Text;
using Clean.Architecture.Web;
using Newtonsoft.Json;
using Xunit;

namespace Clean.Architecture.FunctionalTests.ControllerApis;

[Collection("Sequential")]
public class ProjectItemMarkComplete : IClassFixture<CustomWebApplicationFactory<WebMarker>>
{
  private readonly HttpClient _client;

  public ProjectItemMarkComplete(CustomWebApplicationFactory<WebMarker> factory)
  {
    _client = factory.CreateClient();
  }

  [Fact]
  public async Task MarksIncompleteItemComplete()
  {
    Guid projectId = Guid.Parse("91C63CEA-7596-4E04-8C5E-880B2B2625A1");
    Guid itemId = Guid.Parse("91C63CEA-7596-4E04-8C5E-880B2B2625A1");

    var jsonContent = new StringContent(JsonConvert.SerializeObject(null), Encoding.UTF8, "application/json");

    var response = await _client.PatchAsync($"api/projects/{projectId}/complete/{itemId}", jsonContent);
    response.EnsureSuccessStatusCode();

    var stringResponse = await response.Content.ReadAsStringAsync();
    Assert.Equal("", stringResponse);
  }
}

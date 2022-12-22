using Ardalis.HttpClientTestExtensions;
using Clean.Architecture.Web;
using Clean.Architecture.Web.Endpoints.ProjectEndpoints;
using Xunit;

namespace Clean.Architecture.FunctionalTests.ApiEndpoints;

[Collection("Sequential")]
public class ProjectGetById : IClassFixture<CustomWebApplicationFactory<WebMarker>>
{
  private readonly HttpClient _client;

  public ProjectGetById(CustomWebApplicationFactory<WebMarker> factory)
  {
    _client = factory.CreateClient();
  }

  [Fact]
  public async Task ReturnsSeedProjectGivenId1()
  {
    var result = await _client.GetAndDeserializeAsync<GetProjectByIdResponse>(GetProjectByIdRequest.BuildRoute(Guid.Parse("91C63CEA-7596-4E04-8C5E-880B2B2625A1")));

    Assert.Equal(Guid.Parse("91C63CEA-7596-4E04-8C5E-880B2B2625A1"), result.Id);
    Assert.Equal(SeedData.TestProject1.Name, result.Name);
    Assert.Equal(3, result.Items.Count);
  }

  [Fact]
  public async Task ReturnsNotFoundGivenId0()
  {
    string route = GetProjectByIdRequest.BuildRoute(Guid.Empty);
    _ = await _client.GetAndEnsureNotFoundAsync(route);
  }
}

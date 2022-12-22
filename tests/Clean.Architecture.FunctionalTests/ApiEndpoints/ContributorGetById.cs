using Ardalis.HttpClientTestExtensions;
using Clean.Architecture.Web;
using Clean.Architecture.Web.Endpoints.ContributorEndpoints;
using Xunit;

namespace Clean.Architecture.FunctionalTests.ApiEndpoints;

[Collection("Sequential")]
public class ContributorGetById : IClassFixture<CustomWebApplicationFactory<WebMarker>>
{
  private readonly HttpClient _client;

  public ContributorGetById(CustomWebApplicationFactory<WebMarker> factory)
  {
    _client = factory.CreateClient();
  }

  [Fact]
  public async Task ReturnsSeedContributorGivenId1()
  {
    var result = await _client.GetAndDeserializeAsync<ContributorRecord>(GetContributorByIdRequest.BuildRoute(Guid.Parse("91C63CEA-7596-4E04-8C5E-880B2B2625A1")));

    Assert.Equal(Guid.Parse("91C63CEA-7596-4E04-8C5E-880B2B2625A1"), result.Id);
    Assert.Equal(SeedData.Contributor1.Name, result.Name);
  }

  [Fact]
  public async Task ReturnsNotFoundGivenId0()
  {
    string route = GetContributorByIdRequest.BuildRoute(Guid.Empty);
    _ = await _client.GetAndEnsureNotFoundAsync(route);
  }
}

using System.Text;
using System.Text.Json;
using Ardalis.HttpClientTestExtensions;
using Clean.Architecture.Application.DTO.ProjectAggregate;
using Clean.Architecture.SharedKernel;
using Clean.Architecture.Web;

using Xunit;

namespace Clean.Architecture.FunctionalTests.ControllerApis;

[Collection("Sequential")]
public class ProjectCreate : IClassFixture<CustomWebApplicationFactory<Program>>
{
  private readonly HttpClient _client;

  public ProjectCreate(CustomWebApplicationFactory<Program> factory)
  {
    _client = factory.CreateClient();
  }

  [Fact]
  public async Task ReturnsOneProject()
  {
    var result = await _client.GetAndDeserializeAsync<IEnumerable<ProjectViewModel>>("/api/projects");

    Assert.Single(result);
    Assert.Contains(result, i => i.Name == SeedData.TestProject1.Name);
  }
  
  //[Fact]
  //public async Task ReturnsOneDashboard()
  //{
  //  var result = await _client.GetAndDeserializeAsync<DashboardDTO>("/api/projects/dashboard");

  //  Assert.NotNull(result);
  //  Assert.True(result.TotalProjects > 1);
  //}

  [Fact]
  public async Task ReturnsDatatablesJSON()
  {
    var jqueryBindingModel = new JQueryDataTablesFormRequest
    {
      Draw = 1,
      Start = 0,
      Length = 10,
      Search = new Search
      {
        Value = "",
        Regex = false
      },
      Order = new List<Order>
      {
        new Order
        {
          Column = 0,
          Dir = "asc"
        }
      },
      Columns = new List<Column>
      {
        new Column
        {
          Data = "id",
          Name = "",
          Orderable = true,
          Search = new Search
          {
            Value = "",
            Regex = false
          },
          Searchable = true
        },
        new Column
        {
          Data = "name",
          Name = "",
          Orderable = true,
          Search = new Search
          {
            Value = "",
            Regex = false
          },
          Searchable = true
        },
        new Column
        {
          Data = "statusDescription",
          Name = "",
          Orderable = true,
          Search = new Search
          {
            Value = "",
            Regex = false
          },
          Searchable = true
        },
        new Column
        {
          Data = "createdBy",
          Name = "",
          Orderable = true,
          Search = new Search
          {
            Value = "",
            Regex = false
          },
          Searchable = true
        },
        new Column
        {
          Data = "formattedCreatedDate",
          Name = "",
          Orderable = true,
          Search = new Search
          {
            Value = "",
            Regex = false
          },
          Searchable = true
        },
      }
    };

    var jsonContent = new StringContent(JsonSerializer.Serialize(jqueryBindingModel), Encoding.UTF8, "application/json");

    var response = await _client.PostAsync("/api/Projects/DatatablesList", jsonContent);

    response.EnsureSuccessStatusCode();

    var result = await response.Content.ReadAsStringAsync();

    var expectedContains2Json = "\",\"priority\":{\"name\":\"";
    var expectedContainsJson = ",\"createdDate\":\"20";
    var expectedEndJson = "}],\"error\":null}";
    Assert.Contains(expectedContainsJson, result);
    Assert.Contains(expectedContains2Json, result);
    Assert.EndsWith(expectedEndJson, result);
  }
}

using System.Text;
using System.Text.Json;

using Ardalis.HttpClientTestExtensions;

using Clean.Architecture.Application.DTO.InventoryModule.ProductAggregate;

using Xunit;

namespace Clean.Architecture.FunctionalTests.ControllerApis;

[Collection("Sequential")]
public class ProductCreate : IClassFixture<CustomWebApplicationFactory<Program>>
{
  private readonly HttpClient _client;

  public ProductCreate(CustomWebApplicationFactory<Program> factory)
  {
    _client = factory.CreateClient();
  }

  [Fact]
  public async Task ReturnsOneProductAfterAdding()
  {
    var productDTO = new ProductBindingModel
    {
      Name = "Test Product",
      Price = 10,
      CreatedBy = "_SYS_",
    };

    var jsonContent = new StringContent(JsonSerializer.Serialize(productDTO), Encoding.UTF8, "application/json");

    var result = await _client.PostAndDeserializeAsync<ProductDTO>("/api/Products", jsonContent);

    Assert.NotNull(result);
    Assert.True(result.Name == productDTO.Name);
  }

  [Fact]
  public async Task ReturnsTrueAfterAddingBulk()
  {
    var productDTOs = new List<ProductBindingModel>
    {
      new ProductBindingModel
      {
        Name = "Test Product 1",
        Price = 10,
        CreatedBy = "_SYS_",
      },
      new ProductBindingModel
      {
        Name = "Test Product 2",
        Price = 15,
        CreatedBy = "_SYS_",
      },
      new ProductBindingModel
      {
        Name = "Test Product 3",
        Price = 20,
        CreatedBy = "_SYS_",
      }
    };

    var jsonContent = new StringContent(JsonSerializer.Serialize(productDTOs), Encoding.UTF8, "application/json");

    var result = await _client.PostAndDeserializeAsync<bool>("/api/Products/Bulk", jsonContent);

    Assert.True(result);
  }

  [Fact]
  public async Task ReturnsManyProductsAfterAddingBulk()
  {
    var productDTOs = new List<ProductBindingModel>
    {
      new ProductBindingModel
      {
        Name = "Test Product 1",
        Price = 10,
        CreatedBy = "_SYS_",
      },
      new ProductBindingModel
      {
        Name = "Test Product 2",
        Price = 15,
        CreatedBy = "_SYS_",
      },
      new ProductBindingModel
      {
        Name = "Test Product 3",
        Price = 20,
        CreatedBy = "_SYS_",
      }
    };

    var jsonContent = new StringContent(JsonSerializer.Serialize(productDTOs), Encoding.UTF8, "application/json");

    var result = await _client.PostAndDeserializeAsync<bool>("/api/Products/Bulk", jsonContent);

    Assert.True(result);

    var result2 = await _client.GetAndDeserializeAsync<IEnumerable<ProductDTO>>("/api/Products");

    Assert.NotNull(result2);
    Assert.Contains(result2, i => i.Name == productDTOs.FirstOrDefault()!.Name);
  }
}

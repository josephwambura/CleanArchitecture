using System.Text;
using System.Text.Json;

using Ardalis.HttpClientTestExtensions;

using Clean.Architecture.Application.DTO.UserManagementModule.ApplicationUserAggregate;
using Clean.Architecture.SharedKernel.Models;
using Clean.Architecture.SharedKernel.Utils;

using Xunit;

namespace Clean.Architecture.FunctionalTests.ControllerApis;

[Collection("Sequential")]
public class ApplicationUserCreate : IClassFixture<CustomWebApplicationFactory<Program>>
{
  private readonly HttpClient _client;

  public ApplicationUserCreate(CustomWebApplicationFactory<Program> factory)
  {
    _client = factory.CreateClient();
  }

  [Fact]
  public async Task ReturnsOneApplicationUserAfterAdding()
  {
    var randomMiddleName = RandomMiddleName();

    var applicationUserDTO = new ApplicationUserBindingModel
    {
      FirstName = "Kelsey",
      MiddleName = randomMiddleName,
      LastName = "Wambura",
      UserName = $"Kelsey{randomMiddleName}",
      Email = $"kelsey.{randomMiddleName}@gmail.com",
      EmailConfirmed = true,
      PhoneNumber = "0112346959",
      PhoneNumberConfirmed = true,
      CreatedBy = "_SYS_",
    };

    var jsonContent = new StringContent(JsonSerializer.Serialize(applicationUserDTO), Encoding.UTF8, "application/json");

    var result = await _client.PostAndDeserializeAsync<ApplicationUserDTO>("/api/ApplicationUsers", jsonContent);

    Assert.NotNull(result);
    Assert.True(result.UserName == applicationUserDTO.UserName);
  }

  [Fact]
  public async Task ReturnsOneApplicationUserAfterAddingViaCreate()
  {
    var randomMiddleName = RandomMiddleName();

    var applicationUserDTO = new ApplicationUserBindingModel
    {
      FirstName = "Kelsey",
      MiddleName = randomMiddleName,
      LastName = "Wambura",
      UserName = $"Kelsey{randomMiddleName}",
      Email = $"kelsey.{randomMiddleName}@gmail.com",
      EmailConfirmed = true,
      PhoneNumber = "0112346957",
      PhoneNumberConfirmed = true,
      CreatedBy = "_SYS_",
    };

    var jsonContent = new StringContent(JsonSerializer.Serialize(applicationUserDTO), Encoding.UTF8, "application/json");

    var result = await _client.PostAndDeserializeAsync<APIResult>("/api/ApplicationUsers/Create", jsonContent);

    Assert.NotNull(result);
    result.Deconstruct(out bool succeeded, out object @object, out IEnumerable<APIError>? errors);

    Assert.True(succeeded);

    if (succeeded)
    {
      //var createUserResponse = JsonSerializer.Deserialize<CreateUserResponse>(@object);

      //Assert.True(Guid.TryParse(createUserResponse?.UserId, out Guid userId));
      //Assert.True(userId != Guid.Empty);
      //Assert.NotNull(createUserResponse.Code);
      //Assert.NotEmpty(createUserResponse.Code);

      //var jsonContent2 = new StringContent(string.Empty, Encoding.UTF8, "application/json");

      //var result2 = await _client.PatchAsync($"/api/ApplicationUsers/{createUserResponse.UserId}/ConfirmEmail/{createUserResponse.Code}", jsonContent2);

      //var response = await result2.Content.ReadAsStringAsync();

      //Assert.NotNull(response);
      //Assert.True(Convert.ToBoolean(response));
    }
    else
    {
      Assert.NotNull(errors);
      Assert.NotEmpty(errors);
    }
  }

  [Fact]
  public async Task ReturnsTrueAfterAddingBulk()
  {
    var randomMiddleName = RandomMiddleName();
    var randomMiddleName2 = RandomMiddleName();
    var randomMiddleName3 = RandomMiddleName();

    var applicationUserDTOs = new List<ApplicationUserBindingModel>
    {
      new ApplicationUserBindingModel
      {
        FirstName = "Kelsey",
        MiddleName = randomMiddleName,
        LastName = "Wambura",
        UserName = $"Kelsey{randomMiddleName}",
        Email = $"kelsey.{randomMiddleName}@gmail.com",
        PhoneNumber = "0726877526",
        CreatedBy = "_SYS_",
      },
      new ApplicationUserBindingModel
      {
        FirstName = "Kelsey",
        MiddleName = randomMiddleName2,
        LastName = "Wambura",
        UserName = $"Kelsey{randomMiddleName2}",
        Email = $"kelsey.{randomMiddleName2}@gmail.com",
        PhoneNumber = "0112346959",
        CreatedBy = "_SYS_",
      },
      new ApplicationUserBindingModel
      {
        FirstName = "Kelsey",
        MiddleName = randomMiddleName3,
        LastName = "Wambura",
        UserName = $"Kelsey{randomMiddleName3}",
        Email = $"kelsey.{randomMiddleName3}@gmail.com",
        PhoneNumber = "0726877526",
        CreatedBy = "_SYS_",
      }
    };

    var jsonContent = new StringContent(JsonSerializer.Serialize(applicationUserDTOs), Encoding.UTF8, "application/json");

    var result = await _client.PostAndDeserializeAsync<bool>("/api/ApplicationUsers/Bulk", jsonContent);

    Assert.True(result);
  }

  [Fact]
  public async Task ReturnsManyApplicationUsersAfterAddingBulk()
  {
    var randomMiddleName = RandomMiddleName();
    var randomMiddleName2 = RandomMiddleName();
    var randomMiddleName3 = RandomMiddleName();

    var applicationUserDTOs = new List<ApplicationUserBindingModel>
    {
      new ApplicationUserBindingModel
      {
        FirstName = "Kelsey",
        MiddleName = randomMiddleName,
        LastName = "Wambura",
        UserName = $"Kelsey{randomMiddleName}",
        Email = $"kelsey.{randomMiddleName}@gmail.com",
        PhoneNumber = "0112346959",
        CreatedBy = "_SYS_",
      },
      new ApplicationUserBindingModel
      {
        FirstName = "Kelsey",
        MiddleName = randomMiddleName2,
        LastName = "Wambura",
        UserName = $"Kelsey{randomMiddleName2}",
        Email = $"kelsey.{randomMiddleName2}@gmail.com",
        PhoneNumber = "0726877526",
        CreatedBy = "_SYS_",
      },
      new ApplicationUserBindingModel
      {
        FirstName = "Kelsey",
        MiddleName = randomMiddleName3,
        LastName = "Wambura",
        UserName = $"Kelsey{randomMiddleName3}",
        Email = $"kelsey.{randomMiddleName3}@gmail.com",
        PhoneNumber = "0112346959",
        CreatedBy = "_SYS_",
      }
    };

    var jsonContent = new StringContent(JsonSerializer.Serialize(applicationUserDTOs), Encoding.UTF8, "application/json");

    var result = await _client.PostAndDeserializeAsync<bool>("/api/ApplicationUsers/Bulk", jsonContent);

    Assert.True(result);

    var result2 = await _client.GetAndDeserializeAsync<IEnumerable<ApplicationUserDTO>>("/api/ApplicationUsers");

    Assert.NotNull(result2);
    Assert.Contains(result2, i => i.UserName == applicationUserDTOs.FirstOrDefault()!.UserName);
  }

  [Fact]
  public async Task ReturnsLoginResult()
  {
    var randomMiddleName = RandomMiddleName();

    var applicationUserDTO = new ApplicationUserBindingModel
    {
      FirstName = "Kelsey",
      MiddleName = randomMiddleName,
      LastName = "Wambura",
      UserName = $"Kelsey{randomMiddleName}",
      Email = $"kelsey.{randomMiddleName}@gmail.com",
      EmailConfirmed = true,
      PhoneNumber = "0726877526",
      PhoneNumberConfirmed = true,
      CreatedBy = "_SYS_",
    };

    var jsonContent = new StringContent(JsonSerializer.Serialize(applicationUserDTO), Encoding.UTF8, "application/json");

    var result = await _client.PostAndDeserializeAsync<APIResult>("/api/ApplicationUsers/Create", jsonContent);

    Assert.NotNull(result);
    result.Deconstruct(out bool succeeded, out object @object, out IEnumerable<APIError>? errors);

    Assert.True(succeeded);

    if (succeeded)
    {
      var createUserResponse = @object as CreateUserResponse;

      Assert.True(Guid.TryParse(createUserResponse?.UserId, out Guid userId));
      Assert.True(userId != Guid.Empty);
      Assert.NotNull(createUserResponse.Code);
      Assert.NotEmpty(createUserResponse.Code);

      var jsonContent2 = new StringContent(string.Empty, Encoding.UTF8, "application/json");

      var result2 = await _client.PatchAsync($"/api/ApplicationUsers/{createUserResponse.UserId}/ConfirmEmail/{createUserResponse.Code}", jsonContent2);

      var response = await result2.Content.ReadAsStringAsync();

      Assert.NotNull(response);
      Assert.True(Convert.ToBoolean(response));

      var accountLoginBindingModel = new AccountLoginBindingModel
      {
        UserName = applicationUserDTO.UserName,
        Email = applicationUserDTO.Email,
        Password = DefaultSettings.Instance.RootPassword,
        RememberMe = true
      };

      var jsonContent3 = new StringContent(JsonSerializer.Serialize(accountLoginBindingModel), Encoding.UTF8, "application/json");

      var result3 = await _client.PostAndDeserializeAsync<bool>("/api/ApplicationUsers/Login", jsonContent3);

      Assert.True(result3);
    }
    else
    {
      Assert.NotNull(errors);
      Assert.NotEmpty(errors);
    }
  }

  private static string RandomMiddleName()
  {
    var random = new Random();

    return $"Githithu{random.Next(99999)}";
  }
}

class CreateUserResponse
{
  public string? UserId { get; set; }
  public string? Code { get; set; }
}

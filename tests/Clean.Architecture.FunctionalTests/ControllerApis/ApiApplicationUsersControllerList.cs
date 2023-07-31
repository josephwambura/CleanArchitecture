using System.Text;

using Ardalis.HttpClientTestExtensions;

using Clean.Architecture.Application.DTO.UserManagementModule.ApplicationUserAggregate;

using System.Text.Json;

using Xunit;
using Clean.Architecture.SharedKernel.Utils;

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
    var applicationUserDTO = new ApplicationUserBindingModel
    {
      FirstName = "Kelsey",
      MiddleName = "Githithu1",
      LastName = "Wambura",
      UserName = "KelseyGithithu1",
      Email = "kelsey.githithu1@gmail.com",
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
    var applicationUserDTO = new ApplicationUserBindingModel
    {
      FirstName = "Kelsey",
      MiddleName = "Githithu1X",
      LastName = "Wambura",
      UserName = "KelseyGithithu1X",
      Email = "kelsey.githithu1X@gmail.com",
      EmailConfirmed = true,
      PhoneNumber = "0112346957",
      PhoneNumberConfirmed = true,
      CreatedBy = "_SYS_",
    };

    var jsonContent = new StringContent(JsonSerializer.Serialize(applicationUserDTO), Encoding.UTF8, "application/json");

    var result = await _client.PostAndDeserializeAsync<CreateUserResponse>("/api/ApplicationUsers/Create", jsonContent);

    Assert.NotNull(result);
    Assert.True(Guid.TryParse(result.UserId, out Guid userId));
    Assert.True(userId != Guid.Empty);
    Assert.NotNull(result.Code);
    Assert.NotEmpty(result.Code);

    var jsonContent2 = new StringContent(string.Empty, Encoding.UTF8, "application/json");

    var result2 = await _client.PatchAsync($"/api/ApplicationUsers/{result.UserId}/ConfirmEmail/{result.Code}", jsonContent2);

    var response = await result2.Content.ReadAsStringAsync();

    Assert.NotNull(response);
    Assert.True(Convert.ToBoolean(response));
  }

  [Fact]
  public async Task ReturnsTrueAfterAddingBulk()
  {
    var applicationUserDTOs = new List<ApplicationUserBindingModel>
    {
      new ApplicationUserBindingModel
      {
        FirstName = "Kelsey",
        MiddleName = "Githithu2",
        LastName = "Wambura",
        UserName = "KelseyGithithu2",
        Email = "kelsey.githithu2@gmail.com",
        PhoneNumber = "0726877526",
        CreatedBy = "_SYS_",
      },
      new ApplicationUserBindingModel
      {
        FirstName = "Kelsey",
        MiddleName = "Githithu3",
        LastName = "Wambura",
        UserName = "KelseyGithithu3",
        Email = "kelsey.githithu3@gmail.com",
        PhoneNumber = "0112346959",
        CreatedBy = "_SYS_",
      },
      new ApplicationUserBindingModel
      {
        FirstName = "Kelsey",
        MiddleName = "Githithu4",
        LastName = "Wambura",
        UserName = "KelseyGithithu4",
        Email = "kelsey.githithu4@gmail.com",
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
    var applicationUserDTOs = new List<ApplicationUserBindingModel>
    {
      new ApplicationUserBindingModel
      {
        FirstName = "Kelsey",
        MiddleName = "Githithu5",
        LastName = "Wambura",
        UserName = "KelseyGithithu5",
        Email = "kelsey.githithu5@gmail.com",
        PhoneNumber = "0112346959",
        CreatedBy = "_SYS_",
      },
      new ApplicationUserBindingModel
      {
        FirstName = "Kelsey",
        MiddleName = "Githithu6",
        LastName = "Wambura",
        UserName = "KelseyGithithu6",
        Email = "kelsey.githithu6@gmail.com",
        PhoneNumber = "0726877526",
        CreatedBy = "_SYS_",
      },
      new ApplicationUserBindingModel
      {
        FirstName = "Kelsey",
        MiddleName = "Githithu7",
        LastName = "Wambura",
        UserName = "KelseyGithithu7",
        Email = "kelsey.githithu7@gmail.com",
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
    var applicationUserDTO = new ApplicationUserBindingModel
    {
      FirstName = "Kelsey",
      MiddleName = "Githithu7X",
      LastName = "Wambura",
      UserName = "KelseyGithithu7X",
      Email = "kelsey.githithu7X@gmail.com",
      EmailConfirmed = true,
      PhoneNumber = "0726877526",
      PhoneNumberConfirmed = true,
      CreatedBy = "_SYS_",
    };

    var jsonContent = new StringContent(JsonSerializer.Serialize(applicationUserDTO), Encoding.UTF8, "application/json");

    var result = await _client.PostAndDeserializeAsync<CreateUserResponse>("/api/ApplicationUsers/Create", jsonContent);

    Assert.NotNull(result);
    Assert.True(Guid.TryParse(result.UserId, out Guid userId));
    Assert.True(userId != Guid.Empty);
    Assert.NotNull(result.Code);
    Assert.NotEmpty(result.Code);

    var jsonContent2 = new StringContent(string.Empty, Encoding.UTF8, "application/json");

    var result2 = await _client.PatchAsync($"/api/ApplicationUsers/{result.UserId}/ConfirmEmail/{result.Code}", jsonContent2);

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
}

class CreateUserResponse
{
  public string? UserId { get; set; }
  public string? Code { get; set; }
}

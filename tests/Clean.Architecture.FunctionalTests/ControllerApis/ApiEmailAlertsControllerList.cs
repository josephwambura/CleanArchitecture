using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

using Ardalis.HttpClientTestExtensions;

using Clean.Architecture.Application.DTO.MessagingModule.EmailAlertAggregate;
using Clean.Architecture.SharedKernel.Utils;

using Xunit;

namespace Clean.Architecture.FunctionalTests.ControllerApis;

[Collection("Sequential")]
public class EmailAlertCreate : IClassFixture<CustomWebApplicationFactory<Program>>
{
  private readonly HttpClient _client;

  public EmailAlertCreate(CustomWebApplicationFactory<Program> factory)
  {
    _client = factory.CreateClient();
  }

  [Fact]
  public async Task ReturnsOneEmailAlertAfterAdding()
  {
    var emailAlertDTO = new EmailAlertBindingModel
    {
      From = DefaultSettings.Instance.RootEmail,
      EmailMessageTo = "test@example.com",
      EmailMessageSubject = "Test email",
      EmailMessageBody = $"This is a test email. Confirm to consent <a href='{HtmlEncoder.Default.Encode("https://localhost/")}'>clicking here</a>.",
      EmailMessageIsBodyHtml = true,
      EmailMessageSecurityCritical = false,
      CreatedBy = "_SYS_",
    };

    var jsonContent = new StringContent(JsonSerializer.Serialize(emailAlertDTO), Encoding.UTF8, "application/json");

    var result = await _client.PostAndDeserializeAsync<EmailAlertDTO>("/api/EmailAlerts", jsonContent);

    Assert.NotNull(result);
    Assert.True(result.EmailMessageSubject == emailAlertDTO.EmailMessageSubject);
  }

  [Fact]
  public async Task ReturnsTrueAfterAddingBulk()
  {
    var emailAlertDTOs = new List<EmailAlertBindingModel>
    {
      new EmailAlertBindingModel
      {
        From = DefaultSettings.Instance.RootEmail,
        EmailMessageTo = "test@example.com",
        EmailMessageSubject = "Test email",
        EmailMessageBody = $"This is a test email. Confirm to consent <a href='{HtmlEncoder.Default.Encode("https://localhost/")}'>clicking here</a>.",
        EmailMessageIsBodyHtml = true,
        EmailMessageSecurityCritical = false,
        CreatedBy = "_SYS_",
      },
      new EmailAlertBindingModel
      {
        From = DefaultSettings.Instance.RootEmail,
        EmailMessageTo = "test@example.com",
        EmailMessageSubject = "Test email",
        EmailMessageBody = $"This is a test email. Confirm to consent <a href='{HtmlEncoder.Default.Encode("https://localhost/")}'>clicking here</a>.",
        EmailMessageIsBodyHtml = true,
        EmailMessageSecurityCritical = false,
        CreatedBy = "_SYS_",
      },
      new EmailAlertBindingModel
      {
        From = DefaultSettings.Instance.RootEmail,
        EmailMessageTo = "test@example.com",
        EmailMessageSubject = "Test email",
        EmailMessageBody = $"This is a test email. Confirm to consent <a href='{HtmlEncoder.Default.Encode("https://localhost/")}'>clicking here</a>.",
        EmailMessageIsBodyHtml = true,
        EmailMessageSecurityCritical = false,
        CreatedBy = "_SYS_",
      }
    };

    var jsonContent = new StringContent(JsonSerializer.Serialize(emailAlertDTOs), Encoding.UTF8, "application/json");

    var result = await _client.PostAndDeserializeAsync<bool>("/api/EmailAlerts/Bulk", jsonContent);

    Assert.True(result);
  }

  [Fact]
  public async Task ReturnsManyEmailAlertsAfterAddingBulk()
  {
    var emailAlertDTOs = new List<EmailAlertBindingModel>
    {
      new EmailAlertBindingModel
      {
        From = DefaultSettings.Instance.RootEmail,
        EmailMessageTo = "test@example.com",
        EmailMessageSubject = "Test email",
        EmailMessageBody = $"This is a test email. Confirm to consent <a href='{HtmlEncoder.Default.Encode("https://localhost/")}'>clicking here</a>.",
        EmailMessageIsBodyHtml = true,
        EmailMessageSecurityCritical = false,
        CreatedBy = "_SYS_",
      },
      new EmailAlertBindingModel
      {
        From = DefaultSettings.Instance.RootEmail,
        EmailMessageTo = "test@example.com",
        EmailMessageSubject = "Test email",
        EmailMessageBody = $"This is a test email. Confirm to consent <a href='{HtmlEncoder.Default.Encode("https://localhost/")}'>clicking here</a>.",
        EmailMessageIsBodyHtml = true,
        EmailMessageSecurityCritical = false,
        CreatedBy = "_SYS_",
      },
      new EmailAlertBindingModel
      {
        From = DefaultSettings.Instance.RootEmail,
        EmailMessageTo = "test@example.com",
        EmailMessageSubject = "Test email",
        EmailMessageBody = $"This is a test email. Confirm to consent <a href='{HtmlEncoder.Default.Encode("https://localhost/")}'>clicking here</a>.",
        EmailMessageIsBodyHtml = true,
        EmailMessageSecurityCritical = false,
        CreatedBy = "_SYS_",
      }
    };

    var jsonContent = new StringContent(JsonSerializer.Serialize(emailAlertDTOs), Encoding.UTF8, "application/json");

    var result = await _client.PostAndDeserializeAsync<bool>("/api/EmailAlerts/Bulk", jsonContent);

    Assert.True(result);

    var result2 = await _client.GetAndDeserializeAsync<IEnumerable<EmailAlertDTO>>("/api/EmailAlerts");

    Assert.NotNull(result2);
    Assert.Contains(result2, i => i.EmailMessageSubject == emailAlertDTOs.FirstOrDefault()!.EmailMessageSubject);
  }
}

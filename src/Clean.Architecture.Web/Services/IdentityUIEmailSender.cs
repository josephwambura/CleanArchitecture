using Clean.Architecture.Application.DTO.MessagingModule.EmailAlertAggregate;

using Microsoft.AspNetCore.Identity.UI.Services;

namespace Clean.Architecture.Web.Services;

public class IdentityUIEmailSender : IEmailSender
{
  private readonly IChannelService _channelService;
  private readonly IUnitOfWork _unitOfWork;
  private readonly ILogger<IdentityUIEmailSender> _logger;
  private readonly Application.Interfaces.IEmailSender _emailSender;
  private readonly IConfiguration _configuration;

  public IdentityUIEmailSender(IChannelService channelService,
    IUnitOfWork unitOfWork,
    ILogger<IdentityUIEmailSender> logger,
    Application.Interfaces.IEmailSender emailSender,
    IConfiguration configuration)
  {
    _channelService = channelService;
    _unitOfWork = unitOfWork;
    _logger = logger;
    _emailSender = emailSender;
    _configuration = configuration;
  }

  public async Task SendEmailAsync(string email, string subject, string htmlMessage)
  {
    var emailSmtpConfiguration = _configuration.GetSection(nameof(EmailSmtpConfiguration)).Get<EmailSmtpConfiguration>() ?? throw new InvalidOperationException("Configuration section 'EmailSmtpConfiguration' not found.");

    if (emailSmtpConfiguration != null)
    {
      if (emailSmtpConfiguration.ByPassSavingToDatabase)
      {
        _emailSender.SendNetworkEmail(emailSmtpConfiguration.SmtpHost!, (int)emailSmtpConfiguration.SmtpPort!, (bool)emailSmtpConfiguration.SmtpEnableSsl!, emailSmtpConfiguration.SmtpUsername!, emailSmtpConfiguration.SmtpPassword!, DefaultSettings.Instance.RootEmail!, email!, subject, htmlMessage, false, null);

        var fromEmail = DefaultSettings.Instance.RootEmail;

        _logger.LogInformation("Sending email to {email} from {fromEmail} with subject {subject}.", email, fromEmail, subject);
      }
    }
    else
    {
      var staticSettingDTO = await _channelService.FindStaticSettingByKeyAsync(DefaultSettings.Instance.EmailMessageFrom!, new ServiceHeader()).ConfigureAwait(false);

      if (staticSettingDTO != null)
      {
        var emailAlertBindingModel = new EmailAlertBindingModel
        {
          From = staticSettingDTO?.Value?.Value,
          EmailMessageTo = email,
          EmailMessageSubject = subject,
          EmailMessageBody = htmlMessage,
          EmailMessageIsBodyHtml = true,
          EmailMessageSecurityCritical = true,
        };

        await _channelService.AddEmailAlertAsync(_unitOfWork.MapTo<EmailAlertDTO>(emailAlertBindingModel), new ServiceHeader()).ConfigureAwait(false);
      }
    }
  }
}

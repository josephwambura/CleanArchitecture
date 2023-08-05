using System.Net.Mail;

using Clean.Architecture.Domain.Interfaces;

namespace Clean.Architecture.Infrastructure;

public class FakeEmailSender : IEmailSender
{
  public Task SendEmailAsync(string to, string from, string subject, string body, CancellationToken cancellationToken)
  {
    return Task.CompletedTask;
  }

  public Task SendEmailAsync(string to, string from, string subject, string body, CancellationToken cancellationToken, bool isBodyHtml = false)
  {
    return Task.CompletedTask;
  }

  public void SendFileEmail(string pickupDirectoryLocation, MailMessage mailMessage)
  {
  }

  public void SendFileEmail(string pickupDirectoryLocation, MailAddress from, MailAddressCollection to, string subject, string body, bool isBodyHtml, List<string>? attachments)
  {

  }

  public void SendFileEmail(string pickupDirectoryLocation, string from, string to, string subject, string body, bool isBodyHtml, List<string>? attachments)
  {

  }

  public void SendFileEmail(string pickupDirectoryLocation, MailAddress from, MailAddressCollection to, MailAddressCollection cc, string subject, string body, bool isBodyHtml, List<string>? attachments)
  {

  }

  public void SendFileEmail(string pickupDirectoryLocation, string from, string to, string cc, string subject, string body, bool isBodyHtml, List<string>? attachments)
  {

  }

  public void SendNetworkEmail(string host, int port, bool enableSsl, string userName, string password, MailMessage mailMessage)
  {

  }

  public void SendNetworkEmail(string host, int port, bool enableSsl, string userName, string password, MailAddress from, MailAddressCollection to, string subject, string body, bool isBodyHtml, List<string>? attachments)
  {

  }

  public void SendNetworkEmail(string host, int port, bool enableSsl, string userName, string password, string from, string to, string subject, string body, bool isBodyHtml, List<string>? attachments)
  {

  }

  public void SendNetworkEmail(string host, int port, bool enableSsl, string userName, string password, MailAddress from, MailAddressCollection to, MailAddressCollection cc, string subject, string body, bool isBodyHtml, List<string>? attachments)
  {

  }

  public void SendNetworkEmail(string host, int port, bool enableSsl, string userName, string password, string from, string to, string cc, string subject, string body, bool isBodyHtml, List<string>? attachments)
  {

  }
}

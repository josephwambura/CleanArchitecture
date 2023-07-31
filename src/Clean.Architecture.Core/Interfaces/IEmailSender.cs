using System.Net.Mail;

namespace Clean.Architecture.Core.Interfaces;

public interface IEmailSender
{
  Task SendEmailAsync(string to, string from, string subject, string body, CancellationToken cancellationToken);

  Task SendEmailAsync(string to, string from, string subject, string body, CancellationToken cancellationToken, bool isBodyHtml = false);

  void SendNetworkEmail(string host, int port, bool enableSsl, string userName, string password, MailMessage mailMessage);

  void SendNetworkEmail(string host, int port, bool enableSsl, string userName, string password, MailAddress from, MailAddressCollection to, string subject, string body, bool isBodyHtml, List<string>? attachments);

  void SendNetworkEmail(string host, int port, bool enableSsl, string userName, string password, string from, string to, string subject, string body, bool isBodyHtml, List<string>? attachments);

  void SendNetworkEmail(string host, int port, bool enableSsl, string userName, string password, MailAddress from, MailAddressCollection to, MailAddressCollection cc, string subject, string body, bool isBodyHtml, List<string>? attachments);

  void SendNetworkEmail(string host, int port, bool enableSsl, string userName, string password, string from, string to, string cc, string subject, string body, bool isBodyHtml, List<string>? attachments);

  void SendFileEmail(string pickupDirectoryLocation, MailMessage mailMessage);

  void SendFileEmail(string pickupDirectoryLocation, MailAddress from, MailAddressCollection to, string subject, string body, bool isBodyHtml, List<string>? attachments);

  void SendFileEmail(string pickupDirectoryLocation, string from, string to, string subject, string body, bool isBodyHtml, List<string>? attachments);

  void SendFileEmail(string pickupDirectoryLocation, MailAddress from, MailAddressCollection to, MailAddressCollection cc, string subject, string body, bool isBodyHtml, List<string>? attachments);

  void SendFileEmail(string pickupDirectoryLocation, string from, string to, string cc, string subject, string body, bool isBodyHtml, List<string>? attachments);
}

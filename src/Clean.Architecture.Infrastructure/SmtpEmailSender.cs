using System.Net;
using System.Net.Mail;

using Clean.Architecture.Domain.Interfaces;

using Microsoft.Extensions.Logging;

namespace Clean.Architecture.Infrastructure;

public class SmtpEmailSender : IEmailSender
{
  private readonly ILogger<SmtpEmailSender> _logger;

  public SmtpEmailSender(ILogger<SmtpEmailSender> logger)
  {
    _logger = logger;
  }

  public async Task SendEmailAsync(string to, string from, string subject, string body, CancellationToken cancellationToken)
  {
    var emailClient = new SmtpClient("localhost");
    var message = new MailMessage
    {
      From = new MailAddress(from),
      Subject = subject,
      Body = body
    };
    message.To.Add(new MailAddress(to));
    await emailClient.SendMailAsync(message, cancellationToken);
    _logger.LogWarning("Sending email to {to} from {from} with subject {subject}.", to, from, subject);
  }

  public async Task SendEmailAsync(string to, string from, string subject, string body, CancellationToken cancellationToken, bool isBodyHtml = false)
  {
    //var emailClient = new SmtpClient("localhost");

    var emailClient = new SmtpClient()
    {
      EnableSsl = true,
      Port = 587,
      Host = "smtp.gmail.com",
      Credentials = new NetworkCredential("githithudanjsef@gmail.com", "tpkwadwqndpssnce"),
    };

    var message = new MailMessage
    {

      From = new MailAddress(from),
      Subject = subject,
      Body = body,
      IsBodyHtml = isBodyHtml


    };

    message.To.Add(new MailAddress(to));

    await emailClient.SendMailAsync(message, cancellationToken);

    _logger.LogWarning("Sending email to {to} from {from} with subject {subject}.", to, from, subject);
  }

  public void SendNetworkEmail(string host, int port, bool enableSsl, string userName, string password, MailMessage mailMessage)
  {
    using (SmtpClient client = new SmtpClient(host, port))
    {
      client.EnableSsl = enableSsl;

      client.Credentials = new NetworkCredential(userName, password);

      client.ServicePoint.MaxIdleTime = 2;

      client.ServicePoint.ConnectionLimit = 1;

      client.Send(mailMessage);
    }
  }

  public void SendNetworkEmail(string host, int port, bool enableSsl, string userName, string password, MailAddress from, MailAddressCollection to, string subject, string body, bool isBodyHtml, List<string>? attachments)
  {
    using (MailMessage mailMessage = new MailMessage())
    {
      mailMessage.From = from;
      mailMessage.Bcc.Add(from);

      foreach (MailAddress mailAddress in to)
        mailMessage.To.Add(mailAddress);

      if (attachments != null)
        if (attachments.Any())
          foreach (var fileName in attachments)
            mailMessage.Attachments.Add(new Attachment(fileName));

      mailMessage.Subject = subject.Replace('\r', ' ').Replace('\n', ' ');

      mailMessage.Body = body;

      mailMessage.IsBodyHtml = isBodyHtml;

      SendNetworkEmail(host, port, enableSsl, userName, password, mailMessage);
    }
  }

  public void SendNetworkEmail(string host, int port, bool enableSsl, string userName, string password, string from, string to, string subject, string body, bool isBodyHtml, List<string>? attachments)
  {
    MailAddressCollection mailAddressCollectionTo = new MailAddressCollection();

    MailAddress mailAddressFrom = new MailAddress(from);

    string[] strToAddresses = to.Replace("; ", ";").Split(char.Parse(";"));

    for (int intIndex = 0; intIndex < strToAddresses.Length; intIndex++)
      if (!string.IsNullOrWhiteSpace(strToAddresses[intIndex]))
        mailAddressCollectionTo.Add(new MailAddress(strToAddresses[intIndex]));

    SendNetworkEmail(host, port, enableSsl, userName, password, mailAddressFrom, mailAddressCollectionTo, subject, body, isBodyHtml, attachments);
  }

  public void SendNetworkEmail(string host, int port, bool enableSsl, string userName, string password, MailAddress from, MailAddressCollection to, MailAddressCollection cc, string subject, string body, bool isBodyHtml, List<string>? attachments)
  {
    using (MailMessage mailMessage = new MailMessage())
    {
      mailMessage.From = from;
      mailMessage.Bcc.Add(from);

      foreach (MailAddress mailAddress in to)
        mailMessage.To.Add(mailAddress);

      foreach (MailAddress mailAddress in cc)
        mailMessage.CC.Add(mailAddress);

      if (attachments != null)
        if (attachments.Any())
          foreach (var fileName in attachments)
            mailMessage.Attachments.Add(new Attachment(fileName));

      mailMessage.Subject = subject;

      mailMessage.Body = body;

      mailMessage.IsBodyHtml = isBodyHtml;

      SendNetworkEmail(host, port, enableSsl, userName, password, mailMessage);
    }
  }

  public void SendNetworkEmail(string host, int port, bool enableSsl, string userName, string password, string from, string to, string cc, string subject, string body, bool isBodyHtml, List<string>? attachments)
  {
    MailAddressCollection mailAddressCollectionTo = new MailAddressCollection();

    MailAddress mailAddressFrom = new MailAddress(from);

    string[] strToAddresses = to.Replace("; ", ";").Split(char.Parse(";"));

    for (int intIndex = 0; intIndex < strToAddresses.Length; intIndex++)
      if (!string.IsNullOrWhiteSpace(strToAddresses[intIndex]))
        mailAddressCollectionTo.Add(new MailAddress(strToAddresses[intIndex]));

    MailAddressCollection mailAddressCollectionCc = new MailAddressCollection();

    string[] strCCAddresses = cc.Replace("; ", ";").Split(char.Parse(";"));

    for (int intIndex = 0; intIndex < strCCAddresses.Length; intIndex++)
      if (!string.IsNullOrWhiteSpace(strCCAddresses[intIndex]))
        mailAddressCollectionCc.Add(new MailAddress(strCCAddresses[intIndex]));

    SendNetworkEmail(host, port, enableSsl, userName, password, mailAddressFrom, mailAddressCollectionTo, mailAddressCollectionCc, subject, body, isBodyHtml, attachments);
  }

  public void SendFileEmail(string pickupDirectoryLocation, MailMessage mailMessage)
  {
    using (SmtpClient client = new SmtpClient())
    {
      client.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;

      client.PickupDirectoryLocation = pickupDirectoryLocation;

      client.Send(mailMessage);
    }
  }

  public void SendFileEmail(string pickupDirectoryLocation, MailAddress from, MailAddressCollection to, string subject, string body, bool isBodyHtml, List<string>? attachments)
  {
    using (MailMessage mailMessage = new MailMessage())
    {
      mailMessage.From = from;
      mailMessage.Bcc.Add(from);

      foreach (MailAddress mailAddress in to)
        mailMessage.To.Add(mailAddress);

      if (attachments != null)
        if (attachments.Any())
          foreach (var fileName in attachments)
            mailMessage.Attachments.Add(new Attachment(fileName));

      mailMessage.Subject = subject.Replace('\r', ' ').Replace('\n', ' ');

      mailMessage.Body = body;

      mailMessage.IsBodyHtml = isBodyHtml;

      SendFileEmail(pickupDirectoryLocation, mailMessage);
    }
  }

  public void SendFileEmail(string pickupDirectoryLocation, string from, string to, string subject, string body, bool isBodyHtml, List<string>? attachments)
  {
    MailAddressCollection mailAddressCollectionTo = new MailAddressCollection();

    MailAddress mailAddressFrom = new MailAddress(from);

    string[] strToAddresses = to.Replace("; ", ";").Split(char.Parse(";"));

    for (int intIndex = 0; intIndex < strToAddresses.Length; intIndex++)
      if (!string.IsNullOrWhiteSpace(strToAddresses[intIndex]))
        mailAddressCollectionTo.Add(new MailAddress(strToAddresses[intIndex]));

    SendFileEmail(pickupDirectoryLocation, mailAddressFrom, mailAddressCollectionTo, subject, body, isBodyHtml, attachments);
  }

  public void SendFileEmail(string pickupDirectoryLocation, MailAddress from, MailAddressCollection to, MailAddressCollection cc, string subject, string body, bool isBodyHtml, List<string>? attachments)
  {
    using (MailMessage mailMessage = new MailMessage())
    {
      mailMessage.From = from;
      mailMessage.Bcc.Add(from);

      foreach (MailAddress mailAddress in to)
        mailMessage.To.Add(mailAddress);

      foreach (MailAddress mailAddress in cc)
        mailMessage.CC.Add(mailAddress);

      if (attachments != null)
        if (attachments.Any())
          foreach (var fileName in attachments)
            mailMessage.Attachments.Add(new Attachment(fileName));

      mailMessage.Subject = subject;

      mailMessage.Body = body;

      mailMessage.IsBodyHtml = isBodyHtml;

      SendFileEmail(pickupDirectoryLocation, mailMessage);
    }
  }

  public void SendFileEmail(string pickupDirectoryLocation, string from, string to, string cc, string subject, string body, bool isBodyHtml, List<string>? attachments)
  {
    MailAddressCollection mailAddressCollectionTo = new MailAddressCollection();

    MailAddress mailAddressFrom = new MailAddress(from);

    string[] strToAddresses = to.Replace("; ", ";").Split(char.Parse(";"));

    for (int intIndex = 0; intIndex < strToAddresses.Length; intIndex++)
      if (!string.IsNullOrWhiteSpace(strToAddresses[intIndex]))
        mailAddressCollectionTo.Add(new MailAddress(strToAddresses[intIndex]));

    MailAddressCollection mailAddressCollectionCc = new MailAddressCollection();

    string[] strCCAddresses = cc.Replace("; ", ";").Split(char.Parse(";"));

    for (int intIndex = 0; intIndex < strCCAddresses.Length; intIndex++)
      if (!string.IsNullOrWhiteSpace(strCCAddresses[intIndex]))
        mailAddressCollectionCc.Add(new MailAddress(strCCAddresses[intIndex]));

    SendFileEmail(pickupDirectoryLocation, mailAddressFrom, mailAddressCollectionTo, mailAddressCollectionCc, subject, body, isBodyHtml, attachments);
  }
}

namespace Clean.Architecture.SharedKernel.Utils;

public sealed class DefaultSettings
{
  public static DefaultSettings Instance => Nested.instance; // use expression-bodied property

  private class Nested // use nested class
  {
    static Nested() { }
    internal static readonly DefaultSettings instance = new DefaultSettings();
  }

  private DefaultSettings()
  {
    RootUser = "su";
    RootEmail = "application@Clean.Architecture.com";
    RootPassword = "H@rd2H@rk!";
    AuditUser = "auditor";
    AuditPassword = "Ch@113nging";
    Password = "yeknod!";
    PasswordQuestion = "Where were you when you first heard about GPT-4?";
    PasswordAnswer = "idk";
    PasswordExpiry = "Password_Expiry";
    PasswordExpiryPeriod = 90;
    TablePrefix = "ca_";
    ApplicationDisplayName = "Clean Architecture";
    MaximumPINAttempts = 5;
    UserNameMaxLength = 75;
    EmailMessageFrom = "EMAIL_FROM";
  }

  public string RootUser { get; private set; }

  public string RootEmail { get; private set; }

  public string RootPassword { get; private set; }

  public string AuditUser { get; private set; }

  public string AuditPassword { get; private set; }

  public string Password { get; private set; }

  public string PasswordQuestion { get; private set; }

  public string PasswordAnswer { get; private set; }

  public int MaximumPINAttempts { get; private set; }

  public string PasswordExpiry { get; private set; }

  public int PasswordExpiryPeriod { get; private set; }

  public string ApplicationDisplayName { get; private set; }

  public string TablePrefix { get; private set; }

  public int UserNameMaxLength { get; private set; }

  public string? EmailMessageFrom { get; set; }
}

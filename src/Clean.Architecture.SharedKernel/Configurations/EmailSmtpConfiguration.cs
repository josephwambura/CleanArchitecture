namespace Clean.Architecture.SharedKernel.Configurations;

public class EmailSmtpConfiguration
{
  public bool ByPassSavingToDatabase { get; set; }

  public string? SmtpHost { get; set; } = default!;

  public int? SmtpPort { get; set; } = default!;

  public bool? SmtpEnableSsl { get; set; } = default!;

  public string? SmtpUsername { get; set; } = default!;

  public string? SmtpPassword { get; set; } = default!;
}

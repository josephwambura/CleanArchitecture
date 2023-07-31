namespace Clean.Architecture.Infrastructure.Data.Config.MessagingModule;

public class EmailAlertConfiguration : IEntityTypeConfiguration<EmailAlert>
{
  public void Configure(EntityTypeBuilder<EmailAlert> builder)
  {
    builder.OwnsOne(p => p.EmailMessage);

    builder.Property(x => x.CreatedBy).HasMaxLength(DefaultSettings.Instance.UserNameMaxLength);

    builder.Property(x => x.ModifiedBy).HasMaxLength(DefaultSettings.Instance.UserNameMaxLength);

    builder.ToTable(Utility.DbTableName<EmailAlert>());
  }
}

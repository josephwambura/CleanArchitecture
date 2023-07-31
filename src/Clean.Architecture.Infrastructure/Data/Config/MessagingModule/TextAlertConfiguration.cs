namespace Clean.Architecture.Infrastructure.Data.Config.MessagingModule;

public class TextAlertConfiguration : IEntityTypeConfiguration<TextAlert>
{
  public void Configure(EntityTypeBuilder<TextAlert> builder)
  {
    builder.OwnsOne(p => p.TextMessage);

    builder.Property(x => x.CreatedBy).HasMaxLength(DefaultSettings.Instance.UserNameMaxLength);

    builder.Property(x => x.ModifiedBy).HasMaxLength(DefaultSettings.Instance.UserNameMaxLength);

    builder.ToTable(Utility.DbTableName<TextAlert>());
  }
}

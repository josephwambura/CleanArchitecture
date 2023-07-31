namespace Clean.Architecture.Infrastructure.Data.Config.MessagingModule;

public class NotificationRecipientConfiguration : IEntityTypeConfiguration<NotificationRecipient>
{
  public void Configure(EntityTypeBuilder<NotificationRecipient> builder)
  {
    builder.Property(p => p.Name)
        .HasMaxLength(100)
        .IsRequired();

    builder.OwnsOne(p => p.Address);

    builder.Property(x => x.CreatedBy).HasMaxLength(DefaultSettings.Instance.UserNameMaxLength);

    builder.Property(x => x.ModifiedBy).HasMaxLength(DefaultSettings.Instance.UserNameMaxLength);

    builder.ToTable(Utility.DbTableName<NotificationRecipient>());
  }
}

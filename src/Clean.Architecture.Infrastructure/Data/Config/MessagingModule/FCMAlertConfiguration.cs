namespace Clean.Architecture.Infrastructure.Data.Config.MessagingModule;

public class FCMAlertConfiguration : IEntityTypeConfiguration<FCMAlert>
{
  public void Configure(EntityTypeBuilder<FCMAlert> builder)
  {
    builder.Property(p => p.Name)
        .HasMaxLength(100)
        .IsRequired();

    builder.Property(p => p.Priority)
      .HasConversion(
          p => p.Value,
          p => PriorityStatus.FromValue(p));

    builder.Property(x => x.CreatedBy).HasMaxLength(DefaultSettings.Instance.UserNameMaxLength);

    builder.Property(x => x.ModifiedBy).HasMaxLength(DefaultSettings.Instance.UserNameMaxLength);

    builder.ToTable(Utility.DbTableName<FCMAlert>());
  }
}

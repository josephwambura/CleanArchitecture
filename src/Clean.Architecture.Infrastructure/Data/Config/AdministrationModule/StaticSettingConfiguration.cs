using Clean.Architecture.Domain.AdministrationModule.StaticSettingAggregate;

namespace Clean.Architecture.Infrastructure.Data.Config.AdministrationModule;

public class StaticSettingConfiguration : IEntityTypeConfiguration<StaticSetting>
{
  public void Configure(EntityTypeBuilder<StaticSetting> builder)
  {
    builder.Property(p => p.Key)
        .HasMaxLength(200)
        .IsRequired();

    builder.Property(x => x.CreatedBy).HasMaxLength(DefaultSettings.Instance.UserNameMaxLength);

    builder.Property(x => x.ModifiedBy).HasMaxLength(DefaultSettings.Instance.UserNameMaxLength);

    builder.ToTable(Utility.DbTableName<StaticSetting>());
  }
}

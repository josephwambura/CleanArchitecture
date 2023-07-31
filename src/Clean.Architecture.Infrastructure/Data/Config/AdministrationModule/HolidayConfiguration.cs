using Clean.Architecture.Core.AdministrationModule.HolidayAggregate;

namespace Clean.Architecture.Infrastructure.Data.Config.AdministrationModule;

public class HolidayConfiguration : IEntityTypeConfiguration<Holiday>
{
  public void Configure(EntityTypeBuilder<Holiday> builder)
  {
    builder.Property(x => x.CreatedBy).HasMaxLength(DefaultSettings.Instance.UserNameMaxLength);

    builder.Property(x => x.ModifiedBy).HasMaxLength(DefaultSettings.Instance.UserNameMaxLength);

    builder.ToTable(Utility.DbTableName<Holiday>());
  }
}

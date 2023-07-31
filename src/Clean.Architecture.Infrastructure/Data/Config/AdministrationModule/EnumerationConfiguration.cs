using Clean.Architecture.Core.AdministrationModule.EnumerationAggregate;

namespace Clean.Architecture.Infrastructure.Data.Config.AdministrationModule;

public class EnumerationConfiguration : IEntityTypeConfiguration<Enumeration>
{
  public void Configure(EntityTypeBuilder<Enumeration> builder)
  {
    builder.Property(x => x.CreatedBy).HasMaxLength(DefaultSettings.Instance.UserNameMaxLength);

    builder.Property(x => x.ModifiedBy).HasMaxLength(DefaultSettings.Instance.UserNameMaxLength);

    builder.ToTable(Utility.DbTableName<Enumeration>());
  }
}

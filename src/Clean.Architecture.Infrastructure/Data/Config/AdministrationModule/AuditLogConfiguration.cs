using Clean.Architecture.Domain.AdministrationModule.AuditLogAggregate;

namespace Clean.Architecture.Infrastructure.Data.Config.AdministrationModule;

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
  public void Configure(EntityTypeBuilder<AuditLog> builder)
  {
    builder.OwnsOne(p => p.TransactionEnvironment);

    builder.Property(x => x.CreatedBy).HasMaxLength(DefaultSettings.Instance.UserNameMaxLength);

    builder.Property(x => x.ModifiedBy).HasMaxLength(DefaultSettings.Instance.UserNameMaxLength);

    builder.ToTable(Utility.DbTableName<AuditLog>());
  }
}

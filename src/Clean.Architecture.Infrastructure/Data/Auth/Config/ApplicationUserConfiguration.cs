using Clean.Architecture.Core.UserManagementModule.ApplicationUserAggregate;

namespace Clean.Architecture.Infrastructure.Data.Auth.Config;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
  public void Configure(EntityTypeBuilder<ApplicationUser> builder)
  {
    builder.OwnsOne(p => p.TransactionEnvironment);

    builder.Property(x => x.CreatedBy).HasMaxLength(DefaultSettings.Instance.UserNameMaxLength);

    builder.Property(x => x.ModifiedBy).HasMaxLength(DefaultSettings.Instance.UserNameMaxLength);
  }
}

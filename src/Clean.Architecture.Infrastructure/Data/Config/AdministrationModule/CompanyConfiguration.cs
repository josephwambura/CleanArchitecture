using Clean.Architecture.Core.AdministrationModule.CompanyAggregate;
using Clean.Architecture.SharedKernel.Utils;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clean.Architecture.Infrastructure.Data.Config.AdministrationModule;

public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
  public void Configure(EntityTypeBuilder<Company> builder)
  {
    builder.Property(p => p.Name)
        .HasMaxLength(100)
        .IsRequired();

    builder.Property(x => x.CreatedBy).HasMaxLength(DefaultSettings.Instance.UserNameMaxLength);

    builder.Property(x => x.ModifiedBy).HasMaxLength(DefaultSettings.Instance.UserNameMaxLength);

    builder.ToTable(Utility.DbTableName<Company>());
  }
}

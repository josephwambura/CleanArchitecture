using Clean.Architecture.Core;

namespace Clean.Architecture.Infrastructure.Data.Config;

public class DashboardConfiguration : IEntityTypeConfiguration<Dashboard>
{
  public void Configure(EntityTypeBuilder<Dashboard> builder)
  {
    builder.HasNoKey();
    builder.ToView($"View_{nameof(Dashboard)}");
    //builder.Property(v => v.TotalCustomers).HasColumnName("Name");
  }
}

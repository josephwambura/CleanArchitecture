using Clean.Architecture.Core.WeatherForecastAggregate;

namespace Clean.Architecture.Infrastructure.Data.Config;

public class WeatherForecastConfiguration : IEntityTypeConfiguration<WeatherForecast>
{
  public void Configure(EntityTypeBuilder<WeatherForecast> builder)
  {
    builder.Property(x => x.CreatedBy).HasMaxLength(DefaultSettings.Instance.UserNameMaxLength);

    builder.Property(x => x.ModifiedBy).HasMaxLength(DefaultSettings.Instance.UserNameMaxLength);

    builder.ToTable(Utility.DbTableName<WeatherForecast>());
  }
}

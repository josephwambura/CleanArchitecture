using System.Data;
using System.Reflection;

using Clean.Architecture.Domain;
using Clean.Architecture.Domain.AdministrationModule.CompanyAggregate;
using Clean.Architecture.Domain.AdministrationModule.EnumerationAggregate;
using Clean.Architecture.Domain.AdministrationModule.HolidayAggregate;
using Clean.Architecture.Domain.AdministrationModule.StaticSettingAggregate;
using Clean.Architecture.Domain.ContributorAggregate;
using Clean.Architecture.Domain.InventoryModule.ProductAggregate;
using Clean.Architecture.Domain.ProjectAggregate;
using Clean.Architecture.Domain.WeatherForecastAggregate;
using Clean.Architecture.Infrastructure.Data.Auth;
using Clean.Architecture.Infrastructure.Extensions;

using Microsoft.Data.SqlClient;

namespace Clean.Architecture.Infrastructure.Data;

public class AppDbContext : DbContext
{
  private readonly IDomainEventDispatcher? _dispatcher;

  public AppDbContext(DbContextOptions<AppDbContext> options,
    IDomainEventDispatcher? dispatcher)
      : base(options)
  {
    _dispatcher = dispatcher;
  }

  public DbSet<WeatherForecast> WeatherForecasts => Set<WeatherForecast>();
  public DbSet<ToDoItem> ToDoItems => Set<ToDoItem>();
  public DbSet<Project> Projects => Set<Project>();
  public DbSet<Contributor> Contributors => Set<Contributor>();
  public DbSet<Dashboard> Dashboards => Set<Dashboard>();

  #region AdministrationModule

  public DbSet<Company> Companies => Set<Company>();
  public DbSet<StaticSetting> StaticSettings => Set<StaticSetting>();
  public DbSet<Holiday> Holidays => Set<Holiday>();
  public DbSet<Enumeration> Enumerations => Set<Enumeration>();

  #endregion

  #region InventoryModule

  public DbSet<Product> Products => Set<Product>();

  #endregion

  #region MessagingModule

  public DbSet<EmailAlert> EmailAlerts => Set<EmailAlert>();
  public DbSet<TextAlert> TextAlerts => Set<TextAlert>();
  public DbSet<FCMAlert> FCMAlerts => Set<FCMAlert>();
  public DbSet<NotificationRecipient> NotificationRecipients => Set<NotificationRecipient>();

  #endregion

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly(), i => !i.Namespace!.StartsWith(typeof(ApplicationDbContext).Namespace!));
  }

  public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
  {
    var result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(ConfigureAwaitOptions.None);

    // ignore events if no dispatcher provided
    if (_dispatcher == null) return result;

    // dispatch events only if save was successful
    var entitiesWithEvents = ChangeTracker.Entries<EntityBase>()
        .Select(e => e.Entity)
        .Where(e => e.DomainEvents.Any())
        .ToArray();

    await _dispatcher.DispatchAndClearEvents(entitiesWithEvents);

    return result;
  }

  public override int SaveChanges()
  {
    return SaveChangesAsync().GetAwaiter().GetResult();
  }

  public bool BulkInsert<T>(IEnumerable<T> data, string tableName, ServiceHeader? serviceHeader = default)
  {
    var result = default(bool);

    // Get the connection string from the context
    var connectionString = Database.GetConnectionString();

    // Create a SqlBulkCopy object with the connection string
    using (var bulkCopy = new SqlBulkCopy(connectionString, SqlBulkCopyOptions.FireTriggers))
    {
      var table = SQLServerExtensions.GenerateDataTable(data, tableName, bulkCopy);

      bulkCopy.WriteToServer(table);

      result = true;
    }

    return result;
  }
}

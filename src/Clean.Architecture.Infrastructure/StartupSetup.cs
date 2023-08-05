using Clean.Architecture.Domain.UserManagementModule.ApplicationUserAggregate;
using Clean.Architecture.Infrastructure.Data.Auth;

using Microsoft.Extensions.DependencyInjection;

namespace Clean.Architecture.Infrastructure;

public static class StartupSetup
{
  public static void AddDbContext(this IServiceCollection services, string connectionString, byte runningEnvironment = 0)
  {
    switch ((RunningEnvironment)runningEnvironment)
    {
      case RunningEnvironment.VisualStudioDebugSQLServer:
      case RunningEnvironment.WindowsServerIIS:
      case RunningEnvironment.DockerContainer:
        services.AddDbContext<AppDbContext>(options =>
              options.UseSqlServer(connectionString));
        break;
      case RunningEnvironment.VisualStudioDebug:
      default:
        services.AddDbContext<AppDbContext>(options =>
              options.UseSqlite(connectionString)); // will be created in web project root
        break;
    }

    services.AddHealthChecks()
      .AddDbContextCheck<AppDbContext>();
  }

  public static void AddAuthDbContext(this IServiceCollection services, string connectionString, byte runningEnvironment = 0)
  {
    switch ((RunningEnvironment)runningEnvironment)
    {
      case RunningEnvironment.VisualStudioDebugSQLServer:
      case RunningEnvironment.WindowsServerIIS:
      case RunningEnvironment.DockerContainer:
        services.AddDbContext<ApplicationDbContext>(options =>
              options.UseSqlServer(connectionString));
        break;
      case RunningEnvironment.VisualStudioDebug:
      default:
        services.AddDbContext<ApplicationDbContext>(options =>
              options.UseSqlite(connectionString)); // will be created in web project root
        break;
    }

    services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

    services.AddHealthChecks()
      .AddDbContextCheck<ApplicationDbContext>();
  }
}

using System.Reflection;

using Clean.Architecture.Domain.UserManagementModule.ApplicationUserAggregate;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Clean.Architecture.Infrastructure.Data.Auth;
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
  private readonly IAuthDomainEventDispatcher? _dispatcher;

  public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
    IAuthDomainEventDispatcher? dispatcher)
      : base(options)
  {
    _dispatcher = dispatcher;
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly(), i => i.Namespace!.StartsWith(typeof(ApplicationDbContext).Namespace!));
  }

  public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
  {
    var result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(ConfigureAwaitOptions.None);

    // ignore events if no dispatcher provided
    if (_dispatcher == null) return result;

    // dispatch events only if save was successful
    var entitiesWithEvents = ChangeTracker.Entries<ApplicationUser>()
        .Select(e => e.Entity)
        .Where(e => e.DomainEvents.Any())
        .ToArray();

    if (entitiesWithEvents != null && entitiesWithEvents.Length > 0)
      await _dispatcher.DispatchAndClearEvents(entitiesWithEvents);

    return result;
  }

  public override int SaveChanges()
  {
    return SaveChangesAsync().GetAwaiter().GetResult();
  }
}

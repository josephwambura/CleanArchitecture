using Clean.Architecture.SharedKernel.Utils;

using Microsoft.EntityFrameworkCore;

namespace Clean.Architecture.SharedKernel.Interfaces;

/// <summary>
/// Convenience methods to retrieve ambient DbContext instances.
/// </summary>
public interface IAmbientDbContextLocator
{
  /// <summary>
  /// If called within the scope of a DbContextScope, gets or creates
  /// the ambient DbContext instance for the provided DbContext type.
  /// 
  /// Otherwise returns null.
  /// </summary>
  TDbContext Get<TDbContext>(ServiceHeader serviceHeader) where TDbContext : DbContext;
}

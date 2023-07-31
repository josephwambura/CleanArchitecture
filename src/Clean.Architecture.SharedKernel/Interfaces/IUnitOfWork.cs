using Clean.Architecture.SharedKernel.Utils;

namespace Clean.Architecture.SharedKernel.Interfaces;

public interface IUnitOfWork : IDisposable
{
  IReadRepository<TEntity>? GetIReadRepository<TEntity>(byte dbConnectionContext = (byte)DbConnectionContext.LIVE) where TEntity : class, IAggregateRoot;

  IRepository<TEntity>? GetIRepository<TEntity>(byte dbConnectionContext = (byte)DbConnectionContext.LIVE) where TEntity : class, IAggregateRoot;

  // Declare the method for saving the changes asynchronously
  Task<bool> SaveAsync(byte dbConnectionContext = (byte)DbConnectionContext.LIVE);

  TDestination MapTo<TDestination>(object source) where TDestination : class;
  List<TDestination> MapListTo<TDestination>(object source) where TDestination : class;
  IQueryable<TDestination> ProjectTo<TSource, TDestination>(IQueryable<TSource> source) where TDestination : class;
}

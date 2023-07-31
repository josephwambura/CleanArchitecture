using Ardalis.Specification.EntityFrameworkCore;

using AutoMapper;

using Clean.Architecture.Infrastructure.Data.Auth;
using Clean.Architecture.SharedKernel.Extensions;
using Clean.Architecture.SharedKernel.Interfaces;
using Clean.Architecture.SharedKernel.Utils;

namespace Clean.Architecture.Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
  private readonly AppDbContext _context;
  private readonly ApplicationDbContext _authContext;
  private readonly IMapper _mapper;
  private readonly Dictionary<Type, object> readRepositories;
  private readonly Dictionary<Type, object> repositories;

  public UnitOfWork(AppDbContext context, ApplicationDbContext authContext, IMapper mapper)
  {
    this._context = context;
    this._authContext = authContext;
    this._mapper = mapper;
    // Initialize the dictionary
    readRepositories = new Dictionary<Type, object>();
    repositories = new Dictionary<Type, object>();
  }

  // Generic method to get or create a repository for a given type
  public IReadRepository<TEntity>? GetIReadRepository<TEntity>(byte dbConnectionContext = (byte)DbConnectionContext.LIVE) where TEntity : class, IAggregateRoot
  {
    // Check if the repository already exists
    if (readRepositories.TryGetValue(typeof(TEntity), out var value))
    {
      // Return the existing repository as an interface
      return value as IReadRepository<TEntity>;
    }
    else
    {
      switch ((DbConnectionContext)dbConnectionContext)
      {
        case DbConnectionContext.Auth:
          // Create a new repository
          var authReadRepository = new AuthEfRepository<TEntity>(_authContext, _mapper);
          // Add it to the dictionary
          readRepositories.Add(typeof(TEntity), authReadRepository);
          // Return the new repository as an interface
          return authReadRepository as IReadRepository<TEntity>;
        case DbConnectionContext.LIVE:
          // Create a new repository
          var readRepository = new EfRepository<TEntity>(_context, _mapper);
          // Add it to the dictionary
          readRepositories.Add(typeof(TEntity), readRepository);
          // Return the new repository as an interface
          return readRepository as IReadRepository<TEntity>;
        case DbConnectionContext.Audit:
        default:
          break;
      }

      return null;
    }
  }
  
  // Generic method to get or create a repository for a given type
  public IRepository<TEntity>? GetIRepository<TEntity>(byte dbConnectionContext = (byte)DbConnectionContext.LIVE) where TEntity : class, IAggregateRoot
  {
    // Check if the repository already exists
    if (repositories.TryGetValue(typeof(TEntity), out var value))
    {
      // Return the existing repository as an interface
      return value as IRepository<TEntity>;
    }
    else
    {
      switch ((DbConnectionContext)dbConnectionContext)
      {
        case DbConnectionContext.Auth:
          // Create a new repository
          var authRepository = new AuthEfRepository<TEntity>(_authContext, _mapper);
          // Add it to the dictionary
          repositories.Add(typeof(TEntity), authRepository);
          // Return the new repository as an interface
          return authRepository as IRepository<TEntity>;
        case DbConnectionContext.LIVE:
          // Create a new repository
          var repository = new EfRepository<TEntity>(_context, _mapper);
          // Add it to the dictionary
          repositories.Add(typeof(TEntity), repository);
          // Return the new repository as an interface
          return repository as IRepository<TEntity>;
        case DbConnectionContext.Audit:
        default:
          break;
      }

      return null;
    }
  }

  public async Task<bool> SaveAsync(byte dbConnectionContext = (byte)DbConnectionContext.LIVE)
  {
    // it returns the number of state entries written to the database.
    // This might be different from the number of rows affected.
    switch ((DbConnectionContext)dbConnectionContext)
    {
      case DbConnectionContext.Auth:
        return await _authContext.SaveChangesAsync() >= 0;
      case DbConnectionContext.LIVE:
        return await _context.SaveChangesAsync() >= 0;
      case DbConnectionContext.Audit:
      default:
        break;
    }
    return false;
  }

  public TDestination MapTo<TDestination>(object source) where TDestination : class
  {
    return source.MapTo<TDestination>(_mapper);
  }
  
  public List<TDestination> MapListTo<TDestination>(object source) where TDestination : class
  {
    return source.MapListTo<TDestination>(_mapper);
  }

  public IQueryable<TDestination> ProjectTo<TSource, TDestination>(IQueryable<TSource> source) where TDestination : class
  {
    return source.ProjectTo<TSource, TDestination>(_mapper);
  }

  private bool disposed = false;

  protected virtual void Dispose(bool disposing)
  {
    if (!this.disposed)
    {
      if (disposing)
      {
        _context.Dispose();
      }
    }
    this.disposed = true;
  }

  public void Dispose()
  {
    Dispose(true);
    GC.SuppressFinalize(this);
  }
}

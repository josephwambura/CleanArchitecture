using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using AutoMapper;

using Clean.Architecture.Infrastructure.Extensions;
using Clean.Architecture.SharedKernel.Extensions;
using Clean.Architecture.SharedKernel.Interfaces;
using Clean.Architecture.SharedKernel.Utils;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Clean.Architecture.Infrastructure.Data;

// inherit from Ardalis.Specification type
public class EfRepository<T> : RepositoryBase<T>, IReadRepository<T>, IRepository<T> where T : class, IAggregateRoot
{
  private readonly AppDbContext _dbContext;
  private readonly IMapper _mapper;

  public EfRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext)
  {
    _dbContext = dbContext;
    _mapper = mapper;
  }

  public async Task<TTarget?> FirstOrDefaultAsync<TTarget>(ISpecification<T> specification, CancellationToken cancellationToken = default) where TTarget : class
  {
    return (await base.FirstOrDefaultAsync(specification, cancellationToken))?.MapTo<TTarget>(_mapper);
  }
  
  public async Task<List<TTarget>?> ListAsync<TTarget>(ISpecification<T> specification, CancellationToken cancellationToken = default) where TTarget : class
  {
    return (await base.ListAsync(specification, cancellationToken))?.MapListTo<TTarget>(_mapper);
  }
  
  public async Task<List<TTarget>?> ListAsync<TTarget>(CancellationToken cancellationToken = default) where TTarget : class
  {
    return (await base.ListAsync(cancellationToken))?.MapListTo<TTarget>(_mapper);
  }

  public IAsyncEnumerable<T> GetAsyncEnumerable(CancellationToken cancellationToken = default)
  {
    return _dbContext.Set<T>().AsAsyncEnumerable<T>();
  }

  public async Task<IEnumerable<T>> DbSetFromSqlAsync(FormattableString sql, CancellationToken cancellationToken = default)
  {
    return await _dbContext.Set<T>().FromSql(sql).ToListAsync(cancellationToken);
  }

  public async Task<IEnumerable<TElement>> DatabaseSqlQueryAsync<TElement>(FormattableString sql, CancellationToken cancellationToken = default)
  {
    return await _dbContext.Database.SqlQuery<TElement>(sql).ToListAsync(cancellationToken);
  }

  public async Task<int> DatabaseExecuteSqlAsync(FormattableString sql, CancellationToken cancellationToken = default)
  {
    return await _dbContext.Database.ExecuteSqlAsync(sql, cancellationToken);
  }

  public async Task<bool> DatabaseBulkInsertAsync(IEnumerable<T> data, string tableName, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default)
  {
    var result = default(bool);

    // Get the connection string from the context
    var connectionString = _dbContext.Database.GetConnectionString();

    // Create a SqlBulkCopy object with the connection string
    using (var bulkCopy = new SqlBulkCopy(connectionString, SqlBulkCopyOptions.FireTriggers))
    {
      var table = SQLServerExtensions.GenerateDataTable(data, tableName, bulkCopy);

      await bulkCopy.WriteToServerAsync(table, cancellationToken);

      result = true;
    }

    return result;
  }

  public IQueryable<T> ApplySpecification(ISpecification<T> specification)
  {
    return SpecificationEvaluator.Default.GetQuery(_dbContext.Set<T>().AsQueryable(), specification);
  }

  public IQueryable<TTarget> ApplySpecification<TTarget>(ISpecification<T> specification) where TTarget : class
  {
    return (this.ApplySpecification(specification)).ProjectTo<T, TTarget>(_mapper);
  }

  public bool IsDatabaseRelational() => _dbContext.Database.IsRelational();
  public bool IsDatabaseSqlite() => _dbContext.Database.IsSqlite();
  public bool IsDatabaseSqlServer() => _dbContext.Database.IsSqlServer();
}

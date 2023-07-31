using Ardalis.Specification;

using Clean.Architecture.SharedKernel.Utils;

namespace Clean.Architecture.SharedKernel.Interfaces;

// from Ardalis.Specification
public interface IRepository<T> : IRepositoryBase<T> where T : class, IAggregateRoot
{
  IAsyncEnumerable<T> GetAsyncEnumerable(CancellationToken cancellationToken = default);

  Task<int> DatabaseExecuteSqlAsync(FormattableString sql, CancellationToken cancellationToken = default);

  Task<bool> DatabaseBulkInsertAsync(IEnumerable<T> data, string tableName, ServiceHeader? serviceHeader = default, CancellationToken cancellationToken = default);
}

using Ardalis.Specification;

namespace Clean.Architecture.SharedKernel.Interfaces;

public interface IReadRepository<T> : IReadRepositoryBase<T> where T : class, IAggregateRoot
{
  Task<TTarget?> FirstOrDefaultAsync<TTarget>(ISpecification<T> specification, CancellationToken cancellationToken = default) where TTarget : class;

  Task<List<TTarget>?> ListAsync<TTarget>(ISpecification<T> specification, CancellationToken cancellationToken = default) where TTarget : class;

  Task<List<TTarget>?> ListAsync<TTarget>(CancellationToken cancellationToken = default) where TTarget : class;

  IQueryable<T> ApplySpecification(ISpecification<T> specification);

  IQueryable<TTarget> ApplySpecification<TTarget>(ISpecification<T> specification) where TTarget : class;

  Task<IEnumerable<T>> DbSetFromSqlAsync(FormattableString sql, CancellationToken cancellationToken = default);

  Task<IEnumerable<TElement>> DatabaseSqlQueryAsync<TElement>(FormattableString sql, CancellationToken cancellationToken = default);

  bool IsDatabaseRelational();

  bool IsDatabaseSqlite();

  bool IsDatabaseSqlServer();
}

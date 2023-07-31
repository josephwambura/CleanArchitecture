using Ardalis.Specification;

using Clean.Architecture.SharedKernel.Extensions;
using Clean.Architecture.SharedKernel.Interfaces;
using Clean.Architecture.SharedKernel.Utils;

using Microsoft.EntityFrameworkCore;

namespace Clean.Architecture.SharedKernel;

public class PaginatedList<T> : List<T>
{
  public int PageIndex { get; private set; }
  public int TotalPages { get; private set; }

  public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
  {
    PageIndex = pageIndex;
    TotalPages = (int)Math.Ceiling(count / (double)pageSize);

    this.AddRange(items);
  }

  public bool HasPreviousPage => PageIndex > 1;

  public bool HasNextPage => PageIndex < TotalPages;

  public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
  {
    var count = await source.CountAsync();
    var items = await source.Skip((pageIndex) * pageSize).Take(pageSize).ToListAsync();
    return new PaginatedList<T>(items, count, pageIndex, pageSize);
  }
}

public class PaginatedIAggregateRootList<T> : List<T> where T : class, IAggregateRoot
{
  public int PageIndex { get; private set; }
  public int TotalPages { get; private set; }

  public PaginatedIAggregateRootList(List<T>? items, int count, int pageIndex, int pageSize)
  {
    PageIndex = pageIndex;
    TotalPages = (int)Math.Ceiling(count / (double)pageSize);

    if (items != null && items!.Count > 0)
      this.AddRange(items);
  }

  public bool HasPreviousPage => PageIndex > 1;

  public bool HasNextPage => PageIndex < TotalPages;

  public static async Task<(PaginatedIAggregateRootList<T>, int)> CreateAsync(IUnitOfWork unitOfWork, ISpecification<T> specification, int pageIndex, int pageSize, CancellationToken cancellationToken)
  {
    var readRepository = unitOfWork.GetIReadRepository<T>((byte)DbConnectionContext.LIVE);

    if (readRepository == null)
    {
      return (new PaginatedIAggregateRootList<T>(null, 0, pageIndex, pageSize), 0);
    }

    var count = await readRepository!.CountAsync(specification, cancellationToken);
    specification.Query.Skip(pageIndex - 1).Take(pageSize);
    var items = await readRepository!.ListAsync(specification, cancellationToken);
    return (new PaginatedIAggregateRootList<T>(items, count, pageIndex, pageSize), count);
  }

  public static async Task<PageCollectionInfo<T>> CreatePageCollectionInfoAsync(IUnitOfWork unitOfWork, ISpecification<T> specification, int pageIndex, int pageSize, CancellationToken cancellationToken)
  {
    var items = await CreateAsync(unitOfWork, specification, pageIndex, pageSize, cancellationToken);

    return items.Item1.ToPageCollectionInfo(items.Item2, pageIndex, pageSize);
  }
}

public class PaginatedIAggregateRootList<T, TTarget> : List<TTarget> where T : class, IAggregateRoot where TTarget : class
{
  public int PageIndex { get; private set; }
  public int TotalPages { get; private set; }

  public PaginatedIAggregateRootList(List<TTarget>? items, int count, int pageIndex, int pageSize)
  {
    PageIndex = pageIndex;
    TotalPages = (int)Math.Ceiling(count / (double)pageSize);

    if (items != null && items.Count > 0)
      this.AddRange(items);
  }

  public bool HasPreviousPage => PageIndex > 1;

  public bool HasNextPage => PageIndex < TotalPages;

  public static async Task<(PaginatedIAggregateRootList<T, TTarget>, int)> CreateAsync(IUnitOfWork unitOfWork, ISpecification<T> specification, int pageIndex, int pageSize, Func<T, TTarget> mapper, CancellationToken cancellationToken)
  {
    var readRepository = unitOfWork.GetIReadRepository<T>((byte)DbConnectionContext.LIVE);

    if (readRepository == null)
    {
      return (new PaginatedIAggregateRootList<T, TTarget>(null, 0, pageIndex, pageSize), 0);
    }

    var count = await readRepository!.CountAsync(specification, cancellationToken);
    specification.Query.Skip(pageIndex).Take(pageSize);
    var items = await readRepository!.ListAsync<TTarget>(specification, cancellationToken);
    //var itemsDTO = items.Select(mapper).ToList();
    return (new PaginatedIAggregateRootList<T, TTarget>(items, count, pageIndex, pageSize), count);
  }

  public static async Task<PageCollectionInfo<TTarget>> CreatePageCollectionInfoAsync(IUnitOfWork unitOfWork, ISpecification<T> specification, int pageIndex, int pageSize, Func<T, TTarget> mapper, CancellationToken cancellationToken)
  {
    var items = await CreateAsync(unitOfWork, specification, pageIndex, pageSize, mapper, cancellationToken);

    return items.Item1.ToPageCollectionInfo(items.Item2, pageIndex, pageSize);
  }
}

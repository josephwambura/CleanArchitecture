namespace Clean.Architecture.SharedKernel.Extensions;

public static class PaginatedListExtensions
{
  public static PageCollectionInfo<T> ToPageCollectionInfo<T>(this List<T> paginatedList, int itemsCount, int pageIndex, int pageSize) where T : class
  {
    return new PageCollectionInfo<T>
    {
      PageCollection = paginatedList,
      ItemsCount = itemsCount,
      PageIndex = pageIndex,
      PageSize = pageSize,
      FilteredItemsCount = paginatedList.Count
    };
  }
}

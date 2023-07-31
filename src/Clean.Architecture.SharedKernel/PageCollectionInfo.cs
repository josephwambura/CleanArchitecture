namespace Clean.Architecture.SharedKernel;

public class PageCollectionInfo<T> where T : class
{
  public int PageIndex { get; set; }

  public int PageSize { get; set; }

  public List<T>? PageCollection { get; set; }

  public int ItemsCount { get; set; }

  public int FilteredItemsCount { get; set; }
}

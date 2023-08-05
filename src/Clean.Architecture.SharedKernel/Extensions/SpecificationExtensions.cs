using Ardalis.Specification;

using Microsoft.EntityFrameworkCore;

namespace Clean.Architecture.SharedKernel.Extensions;

public static class SpecificationExtensions
{
  public static void ApplyOrdering<T>(this ISpecificationBuilder<T> query, string sortColumn, string sortDirection)
  {
    if (string.IsNullOrWhiteSpace(sortColumn))
    {
      sortColumn = "CreatedDate";
      //throw new ArgumentNullException(nameof(sortColumn));
    }

    if (string.IsNullOrWhiteSpace(sortDirection))
    {
      sortDirection = "desc";
      //throw new ArgumentNullException(nameof(sortDirection));
    }

    if (sortColumn == "FormattedCreatedDate")
      sortColumn = "CreatedDate";
    
    if (sortColumn == "FormattedModifiedDate")
      sortColumn = "ModifiedDate";

    if (sortDirection == "asc")
    {
      query.OrderBy(T => EF.Property<object>(T!, sortColumn));
    }
    else if (sortDirection == "desc")
    {
      query.OrderByDescending(T => EF.Property<object>(T!, sortColumn));
    }
    else
    {
      throw new ArgumentException("Invalid sort direction", nameof(sortDirection));
    }
  }
}

using Ardalis.Specification;

using Clean.Architecture.SharedKernel.Extensions;

namespace Clean.Architecture.Core.WeatherForecastAggregate.Specifications;

public class WeatherForecastsWithFiltersAndInPageSpec : Specification<WeatherForecast>, ISingleResultSpecification
{
  public WeatherForecastsWithFiltersAndInPageSpec(string? searchString, string sortColumn, string sortDirection)
  {
    Query.ApplyOrdering(sortColumn, sortDirection);

    //Query
    //    .EnableCache(nameof(WeatherForecastsWithFiltersAndInPageSpec));
  }
}

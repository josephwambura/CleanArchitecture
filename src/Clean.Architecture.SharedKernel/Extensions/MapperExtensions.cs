using AutoMapper;

namespace Clean.Architecture.SharedKernel.Extensions;

public static class MapperExtensions
{
  public static TDestination MapTo<TDestination>(this object source, IMapper mapper) where TDestination : class
  {
    return mapper.Map<TDestination>(source);
  }
  
  public static List<TDestination> MapListTo<TDestination>(this object source, IMapper mapper) where TDestination : class
  {
    return mapper.Map<List<TDestination>>(source);
  }
  
  public static IQueryable<TDestination> ProjectTo<TSource, TDestination>(this IQueryable<TSource> source, IMapper mapper) where TDestination : class
  {
    return mapper.ProjectTo<TDestination>(source);
  }
}

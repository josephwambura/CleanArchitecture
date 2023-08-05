using AutoMapper;
using Clean.Architecture.Domain.WeatherForecastAggregate;

namespace Clean.Architecture.Application.DTO.WeatherForecastAggregate;

public class WeatherForecastModuleProfile : Profile
{
  public WeatherForecastModuleProfile()
  {
    CreateMap<WeatherForecast, WeatherForecastDTO>().ReverseMap();
  }
}

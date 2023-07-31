
using AutoMapper;

using Clean.Architecture.Application.DTO.AdministrationModule.CompanyAggregate;
using Clean.Architecture.Application.DTO.AdministrationModule.StaticSettingAggregate;
using Clean.Architecture.Core.AdministrationModule.CompanyAggregate;
using Clean.Architecture.Core.AdministrationModule.StaticSettingAggregate;

namespace Clean.Architecture.Application.DTO.AdministrationModule;

public class AdministrationModuleProfile : Profile
{
  public AdministrationModuleProfile()
  {
    CreateMap<Company, CompanyDTO>().ReverseMap();
    CreateMap<StaticSetting, StaticSettingDTO>().ReverseMap();
  }
}

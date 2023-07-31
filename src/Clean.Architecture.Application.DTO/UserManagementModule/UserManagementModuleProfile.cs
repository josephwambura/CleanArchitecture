using AutoMapper;

using Clean.Architecture.Application.DTO.UserManagementModule.ApplicationUserAggregate;
using Clean.Architecture.Core.UserManagementModule.ApplicationUserAggregate;

namespace Clean.Architecture.Application.DTO.UserManagementModule;

public class UserManagementModuleProfile : Profile
{
  public UserManagementModuleProfile()
  {
    CreateMap<ApplicationUser, ApplicationUserDTO>().ReverseMap();
    CreateMap<ApplicationUserBindingModel, ApplicationUserDTO>().ReverseMap();
  }
}

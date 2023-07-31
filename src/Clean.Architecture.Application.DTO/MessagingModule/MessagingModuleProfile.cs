using AutoMapper;

using Clean.Architecture.Application.DTO.MessagingModule.EmailAlertAggregate;
using Clean.Architecture.Application.DTO.MessagingModule.TextAlertAggregate;
using Clean.Architecture.Core.MessagingModule.EmailAlertAggregate;
using Clean.Architecture.Core.MessagingModule.TextAlertAggregate;

namespace Clean.Architecture.Application.DTO.MessagingModule;

public class MessagingModuleProfile : Profile
{
  public MessagingModuleProfile()
  {
    #region EmailAlerts

    CreateMap<EmailAlert, EmailAlertDTO>().ReverseMap();
    CreateMap<EmailAlert, EmailAlertBindingModel>().ReverseMap();
    CreateMap<EmailAlertDTO, EmailAlertBindingModel>().ReverseMap();
    CreateMap<EmailAlert, EmailAlertViewModel>().ReverseMap();
    CreateMap<EmailAlertDTO, EmailAlertViewModel>().ReverseMap();

    #endregion

    #region TextAlerts

    CreateMap<TextAlert, TextAlertDTO>().ReverseMap();
    CreateMap<TextAlert, TextAlertBindingModel>().ReverseMap();
    CreateMap<TextAlertDTO, TextAlertBindingModel>().ReverseMap();
    CreateMap<TextAlert, TextAlertViewModel>().ReverseMap();
    CreateMap<TextAlertDTO, TextAlertViewModel>().ReverseMap();

    #endregion

  }
}


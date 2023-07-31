using AutoMapper;

using Clean.Architecture.Application.DTO.ContributorAggregate;
using Clean.Architecture.Core;
using Clean.Architecture.Core.ContributorAggregate;
using Clean.Architecture.Core.ProjectAggregate;

namespace Clean.Architecture.Application.DTO.ProjectAggregate;

public class ProjectModuleProfile : Profile
{
  public ProjectModuleProfile()
  {
    CreateMap<Project, ProjectDTO>().ReverseMap();
    CreateMap<Project, ProjectViewModel>().ReverseMap();
    CreateMap<ProjectDTO, ProjectViewModel>().ReverseMap();
    CreateMap<ProjectBindingModel, ProjectDTO>().ReverseMap();
    CreateMap<ProjectBindingModel, ProjectViewModel>().ReverseMap();

    CreateMap<ToDoItem, ToDoItemDTO>().ReverseMap();
    CreateMap<ToDoItem, ToDoItemViewModel>().ReverseMap();
    CreateMap<ToDoItemDTO, ToDoItemViewModel>().ReverseMap();

    CreateMap<Contributor, ContributorDTO>().ReverseMap();
    CreateMap<Dashboard, DashboardDTO>().ReverseMap();
  }
}

using AutoMapper;
using Clean.Architecture.Application.DTO.InventoryModule.ProductAggregate;
using Clean.Architecture.Core.InventoryModule.ProductAggregate;

namespace Clean.Architecture.Application.DTO.InventoryModule;

public class InventoryModuleProfile : Profile
{
  public InventoryModuleProfile()
  {
    CreateMap<Product, ProductDTO>().ReverseMap();
    CreateMap<ProductBindingModel, ProductDTO>().ReverseMap();
  }
}

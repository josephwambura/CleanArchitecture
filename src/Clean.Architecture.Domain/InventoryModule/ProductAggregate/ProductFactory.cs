using Clean.Architecture.SharedKernel.Extensions;
using Clean.Architecture.SharedKernel.Utils;

namespace Clean.Architecture.Domain.InventoryModule.ProductAggregate;

public static class ProductFactory
{
  public static Product CreateProduct(string name, decimal price, byte recordStatus, ServiceHeader serviceHeader, Product? original = null)
  {
    var entity = original ?? new Product();

    entity.Name = name;
    entity.Price = price;

    EntityFactoryExtensions.BaseEntityDetails(entity, serviceHeader, original);

    return entity;
  }
}

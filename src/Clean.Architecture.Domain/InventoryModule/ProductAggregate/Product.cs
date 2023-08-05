using Clean.Architecture.SharedKernel;
using Clean.Architecture.SharedKernel.Interfaces;

namespace Clean.Architecture.Domain.InventoryModule.ProductAggregate;

public class Product : EntityBase, IAggregateRoot
{
  public string Name { get; set; } = default!;
  public decimal Price { get; set; }
}

using Clean.Architecture.SharedKernel;

namespace Clean.Architecture.Core.InventoryModule.ProductAggregate.Events;

public class ProductAddedEvent : DomainEventBase
{
  public Product Product { get; set; }

  public ProductAddedEvent(Product product)
  {
    Product = product;
  }
}

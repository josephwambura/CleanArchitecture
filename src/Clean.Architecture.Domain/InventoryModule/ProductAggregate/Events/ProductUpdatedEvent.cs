using Clean.Architecture.SharedKernel;

namespace Clean.Architecture.Domain.InventoryModule.ProductAggregate.Events;

public class ProductUpdatedEvent : DomainEventBase
{
  public Product Product { get; set; }

  public ProductUpdatedEvent(Product product)
  {
    Product = product;
  }
}

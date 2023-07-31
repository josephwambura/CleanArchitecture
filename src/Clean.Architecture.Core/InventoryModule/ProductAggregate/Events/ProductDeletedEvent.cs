using Clean.Architecture.SharedKernel;

namespace Clean.Architecture.Core.InventoryModule.ProductAggregate.Events;

public class ProductDeletedEvent : DomainEventBase
{
  public Guid ProductId { get; set; }

  public ProductDeletedEvent(Guid productId)
  {
    ProductId = productId;
  }
}

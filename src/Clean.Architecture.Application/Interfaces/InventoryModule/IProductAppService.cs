using Ardalis.Result;

using Clean.Architecture.Application.DTO.InventoryModule.ProductAggregate;
using Clean.Architecture.SharedKernel;
using Clean.Architecture.SharedKernel.Utils;

namespace Clean.Architecture.Application.Interfaces.InventoryModule;

public interface IProductAppService
{
  Task<Result<ProductDTO>> AddProductAsync(ProductDTO productDTO, ServiceHeader serviceHeader, CancellationToken cancellationToken = default);

  Task<Result<bool>> BulkInsertProductsAsync(List<ProductDTO> productDTOs, ServiceHeader serviceHeader, CancellationToken cancellationToken = default);

  Task<Result> UpdateProductAsync(Guid productId, ProductDTO productDTO, ServiceHeader serviceHeader, CancellationToken cancellationToken = default);

  Task<Result> DeleteProductAsync(Guid productId, ServiceHeader serviceHeader, CancellationToken cancellationToken = default);

  Task<Result<ProductDTO>> FindProductAsync(Guid productId, ServiceHeader serviceHeader, CancellationToken cancellationToken = default);

  Task<Result<List<ProductDTO>>> FindProductsAsync(ServiceHeader serviceHeader, CancellationToken cancellationToken = default);

  Task<Result<PageCollectionInfo<ProductDTO>>> GetProductsWithFiltersAndInPageAsync(string? searchString, int pageNumber, int pageSize, string sortColumn, string sortDirection, ServiceHeader serviceHeader, CancellationToken cancellationToken = default);
}

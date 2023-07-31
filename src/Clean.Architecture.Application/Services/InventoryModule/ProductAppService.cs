using Ardalis.Result;

using Clean.Architecture.Application.DTO.InventoryModule.ProductAggregate;
using Clean.Architecture.Application.Interfaces.InventoryModule;
using Clean.Architecture.Core.InventoryModule.ProductAggregate;
using Clean.Architecture.Core.InventoryModule.ProductAggregate.Events;
using Clean.Architecture.Core.InventoryModule.ProductAggregate.Specifications;
using Clean.Architecture.SharedKernel;
using Clean.Architecture.SharedKernel.Interfaces;
using Clean.Architecture.SharedKernel.Utils;

using MediatR;

namespace Clean.Architecture.Application.Services.InventoryModule;

public class ProductAppService : IProductAppService
{
  private readonly IUnitOfWork _unitOfWork;
  private readonly IReadRepository<Product>? _readRepository;
  private readonly IRepository<Product>? _repository;
  private readonly IMediator _mediator;

  public ProductAppService(IUnitOfWork unitOfWork, IMediator mediator)
  {
    _unitOfWork = unitOfWork;
    _mediator = mediator;

    _readRepository = _unitOfWork.GetIReadRepository<Product>((byte)DbConnectionContext.LIVE);
    _repository = _unitOfWork.GetIRepository<Product>((byte)DbConnectionContext.LIVE);
  }

  public async Task<Result<ProductDTO>> AddProductAsync(ProductDTO productDTO, ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      var aggregateToAdd = ProductFactory.CreateProduct(productDTO.Name, productDTO.Price, (byte)RecordStatus.Approved, serviceHeader);

      var result = await _repository!.AddAsync(aggregateToAdd, cts.Token);
      await _unitOfWork.SaveAsync();
      var domainEvent = new ProductAddedEvent(result);
      await _mediator.Publish(domainEvent, cts.Token).ConfigureAwait(false);
      return Result.Success(_unitOfWork.MapTo<ProductDTO>(result));
    }
    catch (Exception ex)
    {
      // TODO: Log details here
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result<bool>> BulkInsertProductsAsync(List<ProductDTO> productDTOs, ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    var result = default(bool);

    try
    {
      if (_readRepository!.IsDatabaseSqlServer())
      {
        var products = new List<Product>();

        productDTOs.ForEach(productDTO =>
        {
          products.Add(ProductFactory.CreateProduct(productDTO.Name, productDTO.Price, (byte)RecordStatus.Approved, serviceHeader));
        });

        result = await _repository!.DatabaseBulkInsertAsync(products, Utility.DbTableName<Product>(), serviceHeader, cts.Token);

        result = await _unitOfWork.SaveAsync();
      }
      else
      {
        productDTOs.ForEach(async productDTO =>
        {
          result = await AddProductAsync(productDTO, serviceHeader, cts.Token) != null;
        });
      }

      return Result.Success(result);
    }
    catch (Exception ex)
    {
      // TODO: Log details here
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result> UpdateProductAsync(Guid productId, ProductDTO productDTO, ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      var aggregateToUpdate = await _repository!.GetByIdAsync(productId, cts.Token);
      if (aggregateToUpdate == null) return Result.NotFound();

      aggregateToUpdate = ProductFactory.CreateProduct(productDTO.Name, productDTO.Price, (byte)RecordStatus.Approved, serviceHeader, aggregateToUpdate);

      await _repository!.UpdateAsync(aggregateToUpdate, cts.Token);
      await _unitOfWork.SaveAsync();
      var domainEvent = new ProductUpdatedEvent(aggregateToUpdate);
      await _mediator.Publish(domainEvent, cts.Token).ConfigureAwait(false);
      return Result.Success();
    }
    catch (Exception ex)
    {
      // TODO: Log details here
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result> DeleteProductAsync(Guid productId, ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      var aggregateToDelete = await _readRepository!.GetByIdAsync(productId, cts.Token);
      if (aggregateToDelete == null) return Result.NotFound();

      await _repository!.DeleteAsync(aggregateToDelete, cts.Token);
      var domainEvent = new ProductDeletedEvent(productId);
      await _mediator.Publish(domainEvent, cts.Token).ConfigureAwait(false);
      return Result.Success();
    }
    catch (Exception ex)
    {
      // TODO: Log details here
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result<ProductDTO>> FindProductAsync(Guid productId, ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      var aggregate = await _readRepository!.GetByIdAsync(productId, cts.Token);

      if (aggregate == null)
      {
        return Result.NotFound();
      }

      return new Result<ProductDTO>(_unitOfWork.MapTo<ProductDTO>(aggregate));
    }
    catch (Exception ex)
    {
      // TODO: Log details here
      return Result.Error(new[] { ex.Message });
    }
  }
  
  public async Task<Result<List<ProductDTO>>> FindProductsAsync(ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      var productDTOs = await _readRepository!.ListAsync<ProductDTO>(cts.Token);

      if (productDTOs == null)
      {
        return Result.NotFound();
      }

      return new Result<List<ProductDTO>>(productDTOs);
    }
    catch (Exception ex)
    {
      // TODO: Log details here
      return Result.Error(new[] { ex.Message });
    }
  }

  public async Task<Result<PageCollectionInfo<ProductDTO>>> GetProductsWithFiltersAndInPageAsync(string? searchString, int pageNumber, int pageSize, string sortColumn, string sortDirection, ServiceHeader serviceHeader, CancellationToken cancellationToken = default)
  {
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
      var allProductSpec = new ProductsWithFiltersAndInPageSpec(searchString, sortColumn, sortDirection);

      var products = await PaginatedIAggregateRootList<Product, ProductDTO>.CreatePageCollectionInfoAsync(_unitOfWork, allProductSpec, pageNumber, pageSize, _unitOfWork.MapTo<ProductDTO>, cts.Token);

      if (products != null)
      {
        if (products.PageCollection == null)
        {
          return Result<PageCollectionInfo<ProductDTO>>.NotFound();
        }

        return new Result<PageCollectionInfo<ProductDTO>>(products);
      }

      return Result<PageCollectionInfo<ProductDTO>>.NotFound("");
    }
    catch (Exception ex)
    {
      // TODO: Log details here
      return Result<PageCollectionInfo<ProductDTO>>.Error(new[] { ex.Message });
    }
  }
}

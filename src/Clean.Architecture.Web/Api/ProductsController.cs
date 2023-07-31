using Ardalis.GuardClauses;

using Clean.Architecture.Application.DTO.InventoryModule.ProductAggregate;

using Microsoft.Extensions.Localization;

namespace Clean.Architecture.Web.Api;

/// <summary>
/// A sample API Controller. Consider using API Endpoints (see Endpoints folder) for a more SOLID approach to building APIs
/// https://github.com/ardalis/ApiEndpoints
/// </summary>
public class ProductsController : BaseApiController
{
  private readonly IChannelService _channelService;
  private readonly IUnitOfWork _unitOfWork;
  private readonly IStringLocalizer<ProductsController> _localizer;
  private readonly ILogger<ProductsController> _logger;

  public ProductsController(IChannelService channelService,
    IUnitOfWork unitOfWork,
    IStringLocalizer<ProductsController> localizer,
    ILogger<ProductsController> logger)
  {
    _channelService = Guard.Against.Null(channelService, nameof(channelService));
    _logger = Guard.Against.Null(logger, nameof(logger));
    _localizer = Guard.Against.Null(localizer, nameof(localizer));
    _unitOfWork = Guard.Against.Null(unitOfWork, nameof(unitOfWork));
  }

  // GET: api/Products
  [HttpGet]
  public async Task<IActionResult> List(CancellationToken cancellationToken)
  {
    var result = await _channelService.FindProductsAsync(this.GetServiceHeader(User), cancellationToken);

    if (result.Status == ResultStatus.Ok)
    {
      return Ok(result.Value);
    }
    else if (result.Status == ResultStatus.Invalid)
    {
      return BadRequest(new
      {
        Title = _localizer["InvalidResponseMessage"],
        Detail = string.Join(',', result.ValidationErrors)
      });
    }
    else if (result.Status == ResultStatus.Error)
    {
      return BadRequest(new
      {
        Title = _localizer["ErrorResponseMessage"],
        Detail = string.Join(',', result.Errors)
      });
    }
    else if (result.Status == ResultStatus.NotFound)
    {
      return NotFound();
    }

    return NoContent();
  }

  // POST: api/Products/DatatablesList
  [HttpPost("DatatablesList")]
  public async Task<IActionResult> DatatablesList([FromBody] JQueryDataTablesFormRequest request, CancellationToken cancellationToken)
  {
    if (request == null)
    {
      return BadRequest();
    }

    var response = this.DataTablesJson(request, new PageCollectionInfo<ProductDTO>());

    // Validate the request model
    if (ModelState.IsValid)
    {
      var result = await _channelService.GetProductsWithFiltersAndInPageAsync(request?.Search?.Value, (int)request?.Start!, (int)request?.Length!, request?.Columns?.ElementAt((int)request?.Order?.FirstOrDefault()?.Column!)?.Name!, request?.Order?.FirstOrDefault()?.Dir!, HtmlHelpers.GetServiceHeader(this.HttpContext, User), cancellationToken);

      if (result.Status == ResultStatus.Ok)
      {
        response = this.DataTablesJson(request!, result.Value);
      }
      else if (result.Status == ResultStatus.Invalid)
      {
        return BadRequest(new
        {
          Title = _localizer["InvalidResponseMessage"],
          Detail = string.Join(',', result.ValidationErrors)
        });
      }
      else if (result.Status == ResultStatus.Error)
      {
        return BadRequest(new
        {
          Title = _localizer["ErrorResponseMessage"],
          Detail = string.Join(',', result.Errors)
        });
      }
      else if (result.Status == ResultStatus.NotFound)
      {
        return NotFound();
      }
    }

    return Ok(response);
  }

  // GET: api/Products
  [HttpGet("{id:Guid}")]
  public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
  {
    var result = await _channelService.FindProductAsync(id, this.GetServiceHeader(User), cancellationToken);

    if (result.Status == ResultStatus.Ok)
    {
      return Ok(result.Value);
    }
    else if (result.Status == ResultStatus.Invalid)
    {
      return BadRequest(new
      {
        Title = _localizer["InvalidResponseMessage"],
        Detail = string.Join(',', result.ValidationErrors)
      });
    }
    else if (result.Status == ResultStatus.Error)
    {
      return BadRequest(new
      {
        Title = _localizer["ErrorResponseMessage"],
        Detail = string.Join(',', result.Errors)
      });
    }
    else if (result.Status == ResultStatus.NotFound)
    {
      return NotFound();
    }

    return NoContent();
  }

  // POST: api/Products
  [HttpPost]
  public async Task<IActionResult> Post([FromBody] ProductBindingModel request, CancellationToken cancellationToken)
  {
    var result = await _channelService.AddProductAsync(_unitOfWork.MapTo<ProductDTO>(request), this.GetServiceHeader(User), cancellationToken);

    if (result.Status == ResultStatus.Ok)
    {
      return Ok(result.Value);
    }
    else if (result.Status == ResultStatus.Invalid)
    {
      return BadRequest(new
      {
        Title = _localizer["InvalidResponseMessage"],
        Detail = string.Join(',', result.ValidationErrors)
      });
    }
    else if (result.Status == ResultStatus.Error)
    {
      return BadRequest(new
      {
        Title = _localizer["ErrorResponseMessage"],
        Detail = string.Join(',', result.Errors)
      });
    }
    else if (result.Status == ResultStatus.NotFound)
    {
      return NotFound();
    }

    return NoContent();
  }

  // POST: api/Products/Bulk
  [HttpPost("Bulk")]
  public async Task<IActionResult> BulkPost([FromBody] List<ProductBindingModel> request, CancellationToken cancellationToken)
  {
    var result = await _channelService.BulkInsertProductsAsync(_unitOfWork.MapListTo<ProductDTO>(request), this.GetServiceHeader(User), cancellationToken);

    if (result.Status == ResultStatus.Ok)
    {
      return Ok(result.Value);
    }
    else if (result.Status == ResultStatus.Invalid)
    {
      return BadRequest(new
      {
        Title = _localizer["InvalidResponseMessage"],
        Detail = string.Join(',', result.ValidationErrors)
      });
    }
    else if (result.Status == ResultStatus.Error)
    {
      return BadRequest(new
      {
        Title = _localizer["ErrorResponseMessage"],
        Detail = string.Join(',', result.Errors)
      });
    }
    else if (result.Status == ResultStatus.NotFound)
    {
      return NotFound();
    }

    return NoContent();
  }

  // PATCH: api/Products/delete/{id}
  [HttpDelete("delete/{id:Guid}")]
  public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
  {
    var result = await _channelService.DeleteProductAsync(id, this.GetServiceHeader(User), cancellationToken);

    if (result.Status == ResultStatus.Ok)
    {
      return Ok();
    }
    else if (result.Status == ResultStatus.Invalid)
    {
      return BadRequest(new
      {
        Title = _localizer["InvalidResponseMessage"],
        Detail = string.Join(',', result.ValidationErrors)
      });
    }
    else if (result.Status == ResultStatus.Error)
    {
      return BadRequest(new
      {
        Title = _localizer["ErrorResponseMessage"],
        Detail = string.Join(',', result.Errors)
      });
    }
    else if (result.Status == ResultStatus.NotFound)
    {
      return NotFound();
    }

    return NoContent();
  }
}

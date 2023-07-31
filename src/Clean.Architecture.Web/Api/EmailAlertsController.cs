using Ardalis.GuardClauses;

using Clean.Architecture.Application.DTO.MessagingModule.EmailAlertAggregate;

using Microsoft.Extensions.Localization;

namespace Clean.Architecture.Web.Api;

/// <summary>
/// A sample API Controller. Consider using API Endpoints (see Endpoints folder) for a more SOLID approach to building APIs
/// https://github.com/ardalis/ApiEndpoints
/// </summary>
public class EmailAlertsController : BaseApiController
{
  private readonly IChannelService _channelService;
  private readonly IUnitOfWork _unitOfWork;
  private readonly IStringLocalizer<EmailAlertsController> _localizer;
  private readonly ILogger<EmailAlertsController> _logger;

  public EmailAlertsController(IChannelService channelService,
    IUnitOfWork unitOfWork,
    IStringLocalizer<EmailAlertsController> localizer,
    ILogger<EmailAlertsController> logger)
  {
    _channelService = Guard.Against.Null(channelService, nameof(channelService));
    _logger = Guard.Against.Null(logger, nameof(logger));
    _localizer = Guard.Against.Null(localizer, nameof(localizer));
    _unitOfWork = Guard.Against.Null(unitOfWork, nameof(unitOfWork));
  }

  // GET: api/EmailAlerts
  [HttpGet]
  public async Task<IActionResult> List(CancellationToken cancellationToken = default)
  {
    var result = await _channelService.FindEmailAlertsAsync(this.GetServiceHeader(User), cancellationToken);

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

  // GET: api/EmailAlerts/Pending
  [HttpGet("Pending")]
  public async Task<IActionResult> PendingList(string searchText = "", int pageSize = 10, CancellationToken cancellationToken = default)
  {
    var dlrStatuses = new byte[]
    {
      (byte)DLRStatus.Pending.Value,
      (byte)DLRStatus.UnKnown.Value,
    };

    var result = await _channelService.GetEmailAlertsByDLRStatusAndWithFiltersAndInPageAsync(dlrStatuses, searchText, pageSize, "CreatedDate", "asc", this.GetServiceHeader(User), cancellationToken);

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

  // POST: api/EmailAlerts/DatatablesList
  [HttpPost("DatatablesList")]
  public async Task<IActionResult> DatatablesList([FromBody] JQueryDataTablesFormRequest request, CancellationToken cancellationToken = default)
  {
    if (request == null)
    {
      return BadRequest();
    }

    var response = this.DataTablesJson(request, new PageCollectionInfo<EmailAlertDTO>());

    // Validate the request model
    if (ModelState.IsValid)
    {
      var result = await _channelService.GetEmailAlertsWithFiltersAndInPageAsync(request?.Search?.Value, (int)request?.Start!, (int)request?.Length!, request?.Columns?.ElementAt((int)request?.Order?.FirstOrDefault()?.Column!)?.Name!, request?.Order?.FirstOrDefault()?.Dir!, HtmlHelpers.GetServiceHeader(this.HttpContext, User), cancellationToken);

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

  // GET: api/EmailAlerts
  [HttpGet("{id:Guid}")]
  public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken = default)
  {
    var result = await _channelService.FindEmailAlertAsync(id, this.GetServiceHeader(User), cancellationToken);

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

  // POST: api/EmailAlerts
  [HttpPost]
  public async Task<IActionResult> Post([FromBody] EmailAlertBindingModel request, CancellationToken cancellationToken = default)
  {
    var result = await _channelService.AddEmailAlertAsync(_unitOfWork.MapTo<EmailAlertDTO>(request), this.GetServiceHeader(User), cancellationToken);

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

  // POST: api/EmailAlerts/Bulk
  [HttpPost("Bulk")]
  public async Task<IActionResult> BulkPost([FromBody] List<EmailAlertBindingModel> request, CancellationToken cancellationToken = default)
  {
    var result = await _channelService.BulkInsertEmailAlertsAsync(_unitOfWork.MapListTo<EmailAlertDTO>(request), this.GetServiceHeader(User), cancellationToken);

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

  // PATCH: api/EmailAlerts/delete/{id}
  [HttpDelete("delete/{id:Guid}")]
  public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
  {
    var result = await _channelService.DeleteEmailAlertAsync(id, this.GetServiceHeader(User), cancellationToken);

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

  // PATCH: api/EmailAlerts/{emailAlertId}/MarkQueued/{itemId}
  [HttpPatch("{emailAlertId:Guid}/MarkQueued/{itemId}")]
  public async Task<IActionResult> MarkQueued(Guid emailAlertId, CancellationToken cancellationToken)
  {
    var result = await _channelService.MarkQueuedEmailAlertAsync(emailAlertId, this.GetServiceHeader(User), cancellationToken);

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

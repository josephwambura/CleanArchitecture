using Ardalis.GuardClauses;

using Clean.Architecture.Application.DTO.ProjectAggregate;
using Clean.Architecture.Core.ProjectAggregate;
using Clean.Architecture.Core.ProjectAggregate.Specifications;

using Microsoft.Extensions.Localization;

namespace Clean.Architecture.Web.Api;

/// <summary>
/// A sample API Controller. Consider using API Endpoints (see Endpoints folder) for a more SOLID approach to building APIs
/// https://github.com/ardalis/ApiEndpoints
/// </summary>
public class ProjectsController : BaseApiController
{
  private readonly IChannelService _channelService;
  private readonly IUnitOfWork _unitOfWork;
  private readonly IStringLocalizer<ProjectsController> _localizer;
  private readonly ILogger<ProjectsController> _logger;

  public ProjectsController(IChannelService channelService,
    IUnitOfWork unitOfWork,
    IStringLocalizer<ProjectsController> localizer,
    ILogger<ProjectsController> logger)
  {
    _channelService = Guard.Against.Null(channelService, nameof(channelService));
    _logger = Guard.Against.Null(logger, nameof(logger));
    _localizer = Guard.Against.Null(localizer, nameof(localizer));
    _unitOfWork = Guard.Against.Null(unitOfWork, nameof(unitOfWork));
  }

  // GET: api/Projects
  [HttpGet]
  public async Task<IActionResult> List(CancellationToken cancellationToken)
  {
    var result = await _channelService.FindProjectsAsync(this.GetServiceHeader(User), cancellationToken);

    if (result.Status == ResultStatus.Ok)
    {
      return Ok(_unitOfWork.MapListTo<ProjectViewModel>(result.Value));
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

  // GET: api/Projects/Dashboard
  [HttpGet("Dashboard")]
  public async Task<IActionResult> Dashboard(CancellationToken cancellationToken)
  {
    var result = await _channelService.FindDashboardsAsync(this.GetServiceHeader(User), cancellationToken);

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

  // GET: api/Projects/Paged3List
  [HttpGet("Paged3List")]
  public async Task<IActionResult> Paged3List(string? searchString, int pageNumber, int pageSize, string sortColumn, string sortDirection, CancellationToken cancellationToken)
  {
    var allProjectSpec = new ProjectsWithFiltersAndInPageSpec(searchString, sortColumn, sortDirection);

    return Ok(await PaginatedIAggregateRootList<Project>.CreatePageCollectionInfoAsync(_unitOfWork, allProjectSpec, pageNumber, pageSize, cancellationToken));
  }

  // GET: api/Projects/Paged2List
  [HttpGet("Paged2List")]
  public async Task<IActionResult> Paged2List(string? searchString, int pageNumber, int pageSize, string sortColumn, string sortDirection, CancellationToken cancellationToken)
  {
    var allProjectSpec = new ProjectsWithFiltersAndInPageSpec(searchString, sortColumn, sortDirection);

    return Ok(await PaginatedIAggregateRootList<Project, ProjectDTO>.CreatePageCollectionInfoAsync(_unitOfWork, allProjectSpec, pageNumber, pageSize, _unitOfWork.MapTo<ProjectDTO>, cancellationToken));
  }

  // GET: api/Projects/PagedList
  [HttpGet("PagedList")]
  public async Task<IActionResult> PagedList(string? searchString, int pageNumber, int pageSize, string sortColumn, string sortDirection, CancellationToken cancellationToken)
  {
    return Ok((await _channelService.GetProjectsWithFiltersAndInPageAsync(searchString, pageNumber, pageSize, sortColumn, sortDirection, this.GetServiceHeader(User), cancellationToken)).Value);
  }

  // POST: api/Projects/DatatablesList
  [HttpPost("DatatablesList")]
  public async Task<IActionResult> DatatablesList([FromBody] JQueryDataTablesFormRequest request, CancellationToken cancellationToken)
  {
    if (request == null)
    {
      return BadRequest();
    }

    var response = this.DataTablesJson(request, new PageCollectionInfo<ProjectDTO>());

    // Validate the request model
    if (ModelState.IsValid)
    {
      var result = await _channelService.GetProjectsWithFiltersAndInPageAsync(request?.Search?.Value, (int)request?.Start!, (int)request?.Length!, request?.Columns?.ElementAt((int)request?.Order?.FirstOrDefault()?.Column!)?.Name!, request?.Order?.FirstOrDefault()?.Dir!, HtmlHelpers.GetServiceHeader(this.HttpContext, User), cancellationToken);

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

  // GET: api/Projects
  [HttpGet("{id:Guid}")]
  public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
  {
    var result = await _channelService.FindProjectAsync(id, this.GetServiceHeader(User), cancellationToken);

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

  // POST: api/Projects
  [HttpPost]
  public async Task<IActionResult> Post([FromBody] ProjectDTO request, CancellationToken cancellationToken)
  {
    var result = await _channelService.AddProjectAsync(request, this.GetServiceHeader(User), cancellationToken);

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

  // PATCH: api/Projects/{projectId}/complete/{itemId}
  [HttpPatch("{projectId:Guid}/complete/{itemId}")]
  public async Task<IActionResult> Complete(Guid projectId, Guid itemId, CancellationToken cancellationToken)
  {
    var result = await _channelService.MarkProjectItemsCompleteAsync(projectId, itemId, this.GetServiceHeader(User), cancellationToken);

    //LocalizedString localizedString = _localizer["GreetingMessage"];

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

  // PATCH: api/Projects/completeall/{id}
  [HttpPatch("completeall/{id:Guid}")]
  public async Task<IActionResult> CompleteAll(Guid id, CancellationToken cancellationToken)
  {
    var result = await _channelService.MarkProjectAllItemsCompleteAsync(id, this.GetServiceHeader(User), cancellationToken);

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

  // PATCH: api/Projects/delete/{id}
  [HttpDelete("delete/{id:Guid}")]
  public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
  {
    var result = await _channelService.DeleteProjectAsync(id, this.GetServiceHeader(User), cancellationToken);

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

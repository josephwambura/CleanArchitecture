using Ardalis.ApiEndpoints;

using Clean.Architecture.Application.DTO.ProjectAggregate;

using Swashbuckle.AspNetCore.Annotations;

namespace Clean.Architecture.Web.Endpoints.ProjectEndpoints;

public class JqueryDatatablesList : EndpointBaseAsync
  .WithRequest<JQueryDataTablesFormRequest>
  .WithActionResult<dynamic>
{
  private readonly IChannelService _channelService;

  public JqueryDatatablesList(IChannelService channelService)
  {
    _channelService = channelService;
  }

  [HttpPost("/Projects/Datatables")]
  [SwaggerOperation(
    Summary = "Gets a list of all projects",
    Description = "Gets a list of all projects",
    OperationId = "Project.Datatables",
    Tags = new[] { "ProjectEndpoints" })
  ]
  public override async Task<ActionResult<dynamic>> HandleAsync(
    [FromBody] JQueryDataTablesFormRequest request,
    CancellationToken cancellationToken = new())
  {
    if (request == null)
    {
      return BadRequest();
    }

    var result = await _channelService.GetProjectsWithFiltersAndInPageAsync(request?.Search?.Value, (int)request?.Start!, (int)request?.Length!, request?.Columns?.ElementAt((int)request?.Order?.FirstOrDefault()?.Column!)?.Name!, request?.Order?.FirstOrDefault()?.Dir!, HtmlHelpers.GetServiceHeader(this.HttpContext, User), cancellationToken);

    var response = HtmlHelpers.DataTablesJson(request!.Draw, new PageCollectionInfo<ProjectDTO>());

    if (result.Status == ResultStatus.Ok)
    {
      response = HtmlHelpers.DataTablesJson(request!.Draw, result.Value);
    }
    else if (result.Status == ResultStatus.Invalid)
    {
      return BadRequest(result.ValidationErrors);
    }
    else if (result.Status == ResultStatus.NotFound)
    {
      return NotFound();
    }

    return Ok(response);
  }
}

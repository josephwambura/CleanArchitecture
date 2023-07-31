
using Clean.Architecture.Application.DTO.ProjectAggregate;

namespace Clean.Architecture.Web.Controllers;

[Route("[controller]")]
public class ProjectController : Controller
{
  private readonly IChannelService _channelService;
  private readonly IUnitOfWork _unitOfWork;

  public ProjectController(IChannelService channelService, IUnitOfWork unitOfWork)
  {
    _channelService = channelService;
    _unitOfWork = unitOfWork;
  }

  // GET project/{id?}
  [HttpGet("{id:Guid}")]
  public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
  {
    var result = await _channelService.FindProjectAsync(id, this.GetServiceHeader(User), cancellationToken);

    var response = new ProjectViewModel();

    if (result.Status == ResultStatus.Ok)
    {
      response = _unitOfWork.MapTo<ProjectViewModel>(result.Value);
    }
    return View("Details", response);
    //return PartialView("_Details", response);
  }
}

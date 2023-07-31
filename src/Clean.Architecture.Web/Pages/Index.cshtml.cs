using Ardalis.GuardClauses;

namespace Clean.Architecture.Web.Pages;
public class IndexModel : PageModel
{
  private readonly ILogger<IndexModel> _logger;
  private readonly IChannelService _channelService;
  private readonly IUnitOfWork _unitOfWork;

  public IndexModel(
      IChannelService channelService,
      ILogger<IndexModel> logger,
      IUnitOfWork unitOfWork)
  {
    _channelService = Guard.Against.Null(channelService, nameof(channelService));
    _logger = Guard.Against.Null(logger, nameof(logger));
    _unitOfWork = Guard.Against.Null(unitOfWork, nameof(unitOfWork));
  }



  public IActionResult OnGet()
  {
    return Page();
  }
}


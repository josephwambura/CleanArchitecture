namespace Clean.Architecture.Web.Pages.Demo;
public class PlaceholderModel : PageModel
{
  private readonly ILogger<PlaceholderModel> _logger;

  public PlaceholderModel(ILogger<PlaceholderModel> logger)
  {
    _logger = logger;
  }

  public void OnGet()
  {
  }
}


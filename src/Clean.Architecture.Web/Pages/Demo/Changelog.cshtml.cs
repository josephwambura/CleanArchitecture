namespace Clean.Architecture.Web.Pages.Demo;
public class ChangelogModel : PageModel
{
  private readonly ILogger<ChangelogModel> _logger;

  public ChangelogModel(ILogger<ChangelogModel> logger)
  {
    _logger = logger;
  }

  public void OnGet()
  {
  }
}


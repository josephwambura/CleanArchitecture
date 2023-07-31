namespace Clean.Architecture.Web.Pages;

public class DatatablesModel : PageModel
{
  private readonly ILogger<DatatablesModel> _logger;

  public DatatablesModel(ILogger<DatatablesModel> logger)
  {
    _logger = logger;
  }

  public void OnGet()
  {

  }
}

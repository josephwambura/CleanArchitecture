using Microsoft.Extensions.Localization;

namespace Clean.Architecture.Web.Pages.UserManagement.ApplicationUsers;

public class IndexModel : PageModel // AuthorizedPageModelBase
{
  private readonly ILogger<IndexModel> _logger;
  private readonly IStringLocalizer<IndexModel> _localizer;

  public IndexModel(ILogger<IndexModel> logger, IStringLocalizer<IndexModel> localizer)
  {
    _logger = logger;
    _localizer = localizer;
  }

  public string? PageTitle { get; set; }

  public void OnGet()
  {
    PageTitle = _localizer["PageTitleApplicationUsers"];
  }
}

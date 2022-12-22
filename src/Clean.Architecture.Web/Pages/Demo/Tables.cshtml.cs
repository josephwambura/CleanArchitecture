using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clean.Architecture.Web.Pages.Demo;
public class TablesModel : PageModel
{
    private readonly ILogger<TablesModel> _logger;

    public TablesModel(ILogger<TablesModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }
}


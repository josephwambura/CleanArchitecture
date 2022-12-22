using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clean.Architecture.Web.Pages.Demo;
public class TabsModel : PageModel
{
    private readonly ILogger<TabsModel> _logger;

    public TabsModel(ILogger<TabsModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }
}


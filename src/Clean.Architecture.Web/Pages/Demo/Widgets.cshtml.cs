using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clean.Architecture.Web.Pages.Demo;
public class WidgetsModel : PageModel
{
    private readonly ILogger<WidgetsModel> _logger;

    public WidgetsModel(ILogger<WidgetsModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }
}


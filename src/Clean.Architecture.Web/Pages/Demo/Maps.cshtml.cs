using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clean.Architecture.Web.Pages.Demo;
public class MapsModel : PageModel
{
    private readonly ILogger<MapsModel> _logger;

    public MapsModel(ILogger<MapsModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }
}


using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clean.Architecture.Web.Pages.Demo;
public class MapFullSizeModel : PageModel
{
    private readonly ILogger<MapFullSizeModel> _logger;

    public MapFullSizeModel(ILogger<MapFullSizeModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }
}


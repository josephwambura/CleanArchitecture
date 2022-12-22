using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clean.Architecture.Web.Pages.Demo;
public class MapsVectorModel : PageModel
{
    private readonly ILogger<MapsVectorModel> _logger;

    public MapsVectorModel(ILogger<MapsVectorModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }
}


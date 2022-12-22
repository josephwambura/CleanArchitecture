using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clean.Architecture.Web.Pages.Demo;
public class OffCanvasModel : PageModel
{
    private readonly ILogger<OffCanvasModel> _logger;

    public OffCanvasModel(ILogger<OffCanvasModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }
}


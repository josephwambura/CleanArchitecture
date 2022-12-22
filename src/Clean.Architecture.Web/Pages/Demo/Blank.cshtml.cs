using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clean.Architecture.Web.Pages.Demo;
public class BlankModel : PageModel
{
    private readonly ILogger<BlankModel> _logger;

    public BlankModel(ILogger<BlankModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }
}


using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clean.Architecture.Web.Pages.Demo;
public class ColorsModel : PageModel
{
    private readonly ILogger<ColorsModel> _logger;

    public ColorsModel(ILogger<ColorsModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }
}


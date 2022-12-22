using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clean.Architecture.Web.Pages.Demo;
public class ModalsModel : PageModel
{
    private readonly ILogger<ModalsModel> _logger;

    public ModalsModel(ILogger<ModalsModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }
}


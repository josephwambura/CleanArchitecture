using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clean.Architecture.Web.Pages.Demo;
public class ChartsModel : PageModel
{
    private readonly ILogger<ChartsModel> _logger;

    public ChartsModel(ILogger<ChartsModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }
}


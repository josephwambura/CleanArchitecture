using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clean.Architecture.Web.Pages.Demo;
public class LightboxModel : PageModel
{
    private readonly ILogger<LightboxModel> _logger;

    public LightboxModel(ILogger<LightboxModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }
}


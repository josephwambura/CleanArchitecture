using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clean.Architecture.Web.Pages.Demo;
public class LicenseModel : PageModel
{
    private readonly ILogger<LicenseModel> _logger;

    public LicenseModel(ILogger<LicenseModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }
}


using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clean.Architecture.Web.Pages.Demo;
public class FAQModel : PageModel
{
    private readonly ILogger<FAQModel> _logger;

    public FAQModel(ILogger<FAQModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }
}


using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clean.Architecture.Web.Pages.Demo;
public class TinymceModel : PageModel
{
    private readonly ILogger<TinymceModel> _logger;

    public TinymceModel(ILogger<TinymceModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }
}


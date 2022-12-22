using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clean.Architecture.Web.Pages.Demo;
public class DatatablesModel : PageModel
{
    private readonly ILogger<DatatablesModel> _logger;

    public DatatablesModel(ILogger<DatatablesModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }
}


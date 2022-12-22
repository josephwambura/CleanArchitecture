using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clean.Architecture.Web.Pages.Demo;
public class DataGridModel : PageModel
{
    private readonly ILogger<DataGridModel> _logger;

    public DataGridModel(ILogger<DataGridModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }
}


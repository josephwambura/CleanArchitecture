using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clean.Architecture.Web.Pages.Demo;
public class ListsModel : PageModel
{
    private readonly ILogger<ListsModel> _logger;

    public ListsModel(ILogger<ListsModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }
}


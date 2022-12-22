using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clean.Architecture.Web.Pages.Demo;
public class SearchResultsModel : PageModel
{
    private readonly ILogger<SearchResultsModel> _logger;

    public SearchResultsModel(ILogger<SearchResultsModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }
}


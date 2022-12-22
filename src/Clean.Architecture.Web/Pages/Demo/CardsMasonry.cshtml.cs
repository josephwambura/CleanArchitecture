using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clean.Architecture.Web.Pages.Demo;
public class CardsMasonryModel : PageModel
{
    private readonly ILogger<CardsMasonryModel> _logger;

    public CardsMasonryModel(ILogger<CardsMasonryModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }
}


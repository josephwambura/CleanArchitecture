using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clean.Architecture.Web.Pages.Demo;
public class CardActionsModel : PageModel
{
    private readonly ILogger<CardActionsModel> _logger;

    public CardActionsModel(ILogger<CardActionsModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }
}


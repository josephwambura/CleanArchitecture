using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clean.Architecture.Web.Pages.Demo;
public class CardsModel : PageModel
{
    private readonly ILogger<CardsModel> _logger;

    public CardsModel(ILogger<CardsModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }
}


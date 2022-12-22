using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clean.Architecture.Web.Pages.Demo;
public class ButtonsModel : PageModel
{
    private readonly ILogger<ButtonsModel> _logger;

    public ButtonsModel(ILogger<ButtonsModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }
}


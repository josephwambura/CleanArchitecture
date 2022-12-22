using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clean.Architecture.Web.Pages.Demo;
public class SettingsPlanModel : PageModel
{
    private readonly ILogger<SettingsPlanModel> _logger;

    public SettingsPlanModel(ILogger<SettingsPlanModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }
}


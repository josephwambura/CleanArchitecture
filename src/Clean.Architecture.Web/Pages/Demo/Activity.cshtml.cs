using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clean.Architecture.Web.Pages.Demo;
public class ActivityModel : PageModel
{
    private readonly ILogger<ActivityModel> _logger;

    public ActivityModel(ILogger<ActivityModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }
}


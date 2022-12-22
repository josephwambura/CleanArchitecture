using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clean.Architecture.Web.Pages.Demo;
public class JobListingModel : PageModel
{
    private readonly ILogger<JobListingModel> _logger;

    public JobListingModel(ILogger<JobListingModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }
}


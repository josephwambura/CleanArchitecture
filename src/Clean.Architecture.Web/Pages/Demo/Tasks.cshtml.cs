using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clean.Architecture.Web.Pages.Demo;
public class TasksModel : PageModel
{
    private readonly ILogger<TasksModel> _logger;

    public TasksModel(ILogger<TasksModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }
}


using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clean.Architecture.Web.Pages.Demo;
public class FormElementsModel : PageModel
{
    private readonly ILogger<FormElementsModel> _logger;

    public FormElementsModel(ILogger<FormElementsModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }
}


using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clean.Architecture.Web.Pages.Demo;
public class InvoiceModel : PageModel
{
    private readonly ILogger<InvoiceModel> _logger;

    public InvoiceModel(ILogger<InvoiceModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }
}


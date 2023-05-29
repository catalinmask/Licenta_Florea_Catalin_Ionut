using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Website.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public IActionResult OnGet()
    {
        if (Request.Cookies.ContainsKey("userEmail"))
        {
            Response.Cookies.Delete("userEmail");
            return RedirectToPage("Index");
        }
        else
        {
            return Page();
        }
    }
}


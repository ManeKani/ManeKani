using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ManeKani.Pages;

public class LoginModel : PageModel
{

    private readonly ILogger<LoginModel> _logger;

    public LoginModel(ILogger<LoginModel> logger)
    {
        _logger = logger;
    }


    public IActionResult OnGet()
    {
        _logger.LogInformation("Login page requested");
        if (string.IsNullOrEmpty(ReturnTo))
        {
            return new RedirectResult($"http://127.0.0.1:4433/self-service/login/browser");
        }
        else
        {
            return new RedirectResult($"http://127.0.0.1:4433/self-service/login/browser?return_to={ReturnTo}");
        }
    }

    [BindProperty(SupportsGet = true)]
    public string? ReturnTo { get; set; }
}
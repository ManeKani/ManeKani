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
        var oryAuthUrl = $"{Request.Scheme}://127.0.0.1:4433/self-service/login/browser";

        if (string.IsNullOrEmpty(ReturnTo))
        {
            return new RedirectResult(oryAuthUrl);
        }
        else
        {
            return new RedirectResult(oryAuthUrl + "?return_to=" + ReturnTo);
        }
    }

    [BindProperty(SupportsGet = true)]
    public string? ReturnTo { get; set; }
}
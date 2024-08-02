using ManeKani.Core.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Humanizer;
using SqlKata.Execution;
using ManeKani.DB;
using Microsoft.AspNetCore.Mvc;

namespace ManeKani.Pages.Settings.ApiKeys;

public class ApiKeysIndexModel : PageModel
{
    private readonly ManeKaniDatabase _db;
    private readonly ILogger<ApiKeysIndexModel> _logger;


    public ApiKeysIndexModel(ILogger<ApiKeysIndexModel> logger, ManeKaniDatabase db)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        // this should always be defined, ensured by the authorization filter
        var userId = HttpContext.User.Identity!.Name!;

        // query user api keys
        var keys = await _db.GetUserApiKeys(Guid.Parse(userId));

        _logger.LogInformation("User {} has {} API keys", userId, keys.Count());

        ApiKeys = [.. keys];

        return Page();
    }

    public PublicApiKey[] ApiKeys { get; set; } = [];

}

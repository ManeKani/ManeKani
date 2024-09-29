using ManeKani.Core.Models;
using ManeKani.Core.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Humanizer;
using SqlKata.Execution;
using ManeKani.DB;
using Microsoft.AspNetCore.Mvc;
using ManeKani.Core.Adapters;

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
        CurrentUser = UsersAdapter.GetUser(_db, Guid.Parse(HttpContext.User.Identity!.Name!)).Result;
        ViewData["CurrentUser"] = CurrentUser;

        // // this should always be defined, ensured by the authorization filter
        // CurrentUser = UsersAdapter.GetUser(_db, Guid.Parse(HttpContext.User.Identity!.Name!)).Result;
        // ViewData["CurrentUser"] = CurrentUser;

        // query user api keys
        var keys = await _db.GetUserApiKeys(CurrentUser.Id);

        _logger.LogInformation("User {} has {} API keys", CurrentUser.Id, keys.Count());

        ApiKeys = [.. keys];

        return Page();
    }

    public PublicApiKey[] ApiKeys { get; set; } = [];

    public User? CurrentUser { get; set; }

}

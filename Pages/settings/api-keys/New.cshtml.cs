using ManeKani.Pages.Shared.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Cryptography;
using SqlKata.Execution;
using ManeKani.Core.Adapters;
using System.Security.Claims;
using ManeKani.Auth.Ory;
using ManeKani.Core.Models;
using ManeKani.DB;

namespace ManeKani.Pages.Settings.ApiKeys;


public class ApiKeysNewModel : PageModel
{
    private readonly ManeKaniDatabase _db;
    private readonly ILogger<ApiKeysNewModel> _logger;

    public ApiKeysNewModel(ILogger<ApiKeysNewModel> logger, ManeKaniDatabase db)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        // this should always be defined, ensured by the authorization guard
        var user = new OryUser(HttpContext.User);


        string? keyName = Request.Form["apikey_name"];
        string? keyPermissions = Request.Form["apikey_permission"];


        var keyClaims = new ApiKeyClaims();
        keyPermissions?.Split(",").Select(x => x.Trim()).ToList().ForEach(x =>
        {
            switch (x)
            {
                case "deck_delete":
                    keyClaims.DeckDelete = true;
                    break;
                case "deck_write":
                    keyClaims.DeckWrite = true;
                    break;
                case "subject_delete":
                    keyClaims.SubjectDelete = true;
                    break;
                case "subject_write":
                    keyClaims.SubjectWrite = true;
                    break;
                case "study_data_delete":
                    keyClaims.StudyDataDelete = true;
                    break;
                case "study_data_write":
                    keyClaims.StudyDataWrite = true;
                    break;
                case "review_create":
                    keyClaims.ReviewCreate = true;
                    break;
            }
        });

        var apiKey = await ApiKeysAdapter.CreateApiKey(_db, user.Id,
            new CreateApiKeyRequest
            {
                Name = keyName ?? "",
                Claims = keyClaims,
            });

        return Partial("Components/_CreateApiKeyForm", new CreateApiKeyFormModel { GeneratedApiKey = apiKey.Key });
    }
}
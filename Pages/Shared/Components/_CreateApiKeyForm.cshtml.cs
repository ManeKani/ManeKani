using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ManeKani.Pages.Shared.Components;

public class CreateApiKeyFormModel
{
    // public CreateApiKeyFormModel()
    // {
    // }

    public string? GeneratedApiKey { get; set; }
    public string? CSRF { get; set; }
}
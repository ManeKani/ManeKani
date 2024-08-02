using Dapper;
using Microsoft.AspNetCore.Mvc;

namespace ManeKani.Pages.Shared.Components;
public class _NavigateBackModel
{
    public required string ReturnUrl { get; set; }

    public BreadCrumb[] BreadCrumbs()
    {
        var parts = ReturnUrl.Split('/').Where((s) => !string.IsNullOrEmpty(s));

        return parts.Select((url, index) => new BreadCrumb
        {
            Text = url,
            Page = $"/{string.Join('/', parts.Take(index + 1))}/index"
        }).ToArray();
    }
}

public class BreadCrumb
{
    public required string Text { get; set; }
    public required string Page { get; set; }
}
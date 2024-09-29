using System.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ManeKani.Pages.Shared.Icons;

public class IconOptions
{
    public int Size { get; set; } = 24;
    public string? Color { get; set; } = "currentColor";

    public string Id { get; }

    public IconOptions()
    {
        Id = $"icon-{Guid.NewGuid():N}";
    }

    public IconOptions(int size)
    {
        Id = $"icon-{Guid.NewGuid():N}";

        Size = size;
    }

    public IconOptions(string color)
    {
        Id = $"icon-{Guid.NewGuid():N}";

        Color = color;
    }

    public IconOptions(int size, string color)
    {
        Id = $"icon-{Guid.NewGuid():N}";

        Size = size;
        Color = color;
    }
}

public static class HtmlHelperExtensions
{
    public static Task<IHtmlContent> Icon(this IHtmlHelper html, string name, IconOptions? options = null)
    {
        options ??= new IconOptions();
        return html.PartialAsync($"Icons/_{name}", options);
    }
}

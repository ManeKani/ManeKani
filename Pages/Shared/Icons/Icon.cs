using System.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ManeKani.Pages.Shared.Icons;

public class IconOptions
{
    public int Size { get; set; } = 24;
    public string? Color { get; set; } = "currentColor";

    public IconOptions()
    {
    }

    public IconOptions(int size)
    {
        Size = size;
    }

    public IconOptions(string color)
    {
        Color = color;
    }

    public IconOptions(int size, string color)
    {
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

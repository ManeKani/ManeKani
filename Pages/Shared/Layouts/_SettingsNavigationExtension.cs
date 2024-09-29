using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ManeKani.Pages.Shared.Layouts;

public static class SettingsNavigationExtension
{
    public static HtmlString SettingsNavigator(
        this IHtmlHelper htmlHelper,
        string text,
        string page,
        string pageModel,
        string classes = ""
    )
    {
        var a = new TagBuilder("a");

        var viewData = htmlHelper.ViewContext.ViewData;
        var settingPage = viewData["Setting"] as string;

        if (string.Equals(settingPage, page, StringComparison.OrdinalIgnoreCase))
        {
            a.AddCssClass("active");
        }

        a.Attributes.Add("href", pageModel);
        a.AddCssClass(classes);
        a.InnerHtml.Append(text);

        return new HtmlString(GetString(a));
    }

    public static string GetString(IHtmlContent content)
    {
        using (var writer = new System.IO.StringWriter())
        {
            content.WriteTo(writer, HtmlEncoder.Default);
            return writer.ToString();
        }
    }
}



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
        var li = new TagBuilder("li");
        li.AddCssClass(classes);

        var viewData = htmlHelper.ViewContext.ViewData;
        var settingPage = viewData["Setting"] as string;

        if (string.Equals(settingPage, page, StringComparison.OrdinalIgnoreCase))
        {
            li.AddCssClass("active");
        }

        var a = new TagBuilder("a");
        a.Attributes.Add("href", pageModel);
        a.InnerHtml.Append(text);
        li.InnerHtml.AppendHtml(a);

        return new HtmlString(GetString(li));
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



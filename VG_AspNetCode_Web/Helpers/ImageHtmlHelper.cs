using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace VG_AspNetCore_Web.Helpers
{
    public static class ImageHtmlHelper
    {
        public static IHtmlContent NorthwindImageLink(this IHtmlHelper htmlHelper,
             int imageId, string labelText)
        {
            return new HtmlString($"<a href='images/{imageId}'>{labelText}</a>");
        }
    }
}
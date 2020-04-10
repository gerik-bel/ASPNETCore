using Microsoft.AspNetCore.Razor.TagHelpers;

namespace VG_AspNetCore_Web.Helpers
{
    [HtmlTargetElement("a", Attributes = "northwind-id")]
    public class ImageTagHelper : TagHelper
    {
        public int NorthwindId { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.SetAttribute("href", $"images/{NorthwindId}");
            base.Process(context, output);
        }
    }
}

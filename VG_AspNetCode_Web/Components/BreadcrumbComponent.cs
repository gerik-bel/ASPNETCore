using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace VG_AspNetCore_Web.Components
{
    public class BreadcrumbComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            //var routes = urlPath.Split('/');
            var action = ViewContext.RouteData.Values["action"].ToString();
            BreadcrumbItem breadcrumbItem = new BreadcrumbItem
            {
                EntityName = ViewContext.RouteData.Values["controller"].ToString()
            };
            switch (action)
            {
                case "Create":
                case "Edit":
                    {
                        breadcrumbItem.Mode = BreadcrumbMode.Edit;
                        break;
                    }
                case "Details":
                case "ImageUpload":
                    breadcrumbItem.Mode = BreadcrumbMode.View;
                    break;
                default:
                    breadcrumbItem.Mode = BreadcrumbMode.Default;
                    break;
            }
            return View(breadcrumbItem);
        }
    }

    public class BreadcrumbItem
    {
        public string EntityName { get; set; }
        public BreadcrumbMode Mode { get; set; }
    }

    public enum BreadcrumbMode
    {
        Default,
        Edit,
        View
    }
}
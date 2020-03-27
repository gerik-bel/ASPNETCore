using Microsoft.AspNetCore.Mvc;
using VG_AspNetCore_Web.Services;

namespace VG_AspNetCore_Web.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoriesService _categoriesService;
        public CategoriesController(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }

        public IActionResult Index()
        {
            var categories = _categoriesService.GetAll();
            return View(categories);
        }
    }
}
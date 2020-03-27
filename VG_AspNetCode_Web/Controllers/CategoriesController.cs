using Microsoft.AspNetCore.Mvc;
using WebApp01Introduction.Services;

namespace WebApp01Introduction.Controllers
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
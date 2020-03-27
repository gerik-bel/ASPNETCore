using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
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

        public async Task<IActionResult> Index()
        {
            var categories = await _categoriesService.GetAllAsync();
            return View(categories);
        }
    }
}
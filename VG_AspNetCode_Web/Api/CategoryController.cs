using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VG_AspNetCore_Web.Services;

namespace VG_AspNetCore_Web.Api
{
    [Route("api/[controller]")]
    public class CategoryController : Controller
    {
        private readonly ICategoriesService _categoriesService;

        public CategoryController(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Get()
        {
            var categories = await _categoriesService.GetAllAsync();
            return Ok(categories);
        }
    }
}
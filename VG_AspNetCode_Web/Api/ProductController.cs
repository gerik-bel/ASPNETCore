using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VG_AspNetCore_Web.Services;

namespace VG_AspNetCore_Web.Api
{
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly IProductsService _productsServiceService;

        public ProductController(IProductsService productsServiceService)
        {
            _productsServiceService = productsServiceService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Get()
        {
            var products = await _productsServiceService.GetAllAsync();
            return Ok(products);
        }
    }
}
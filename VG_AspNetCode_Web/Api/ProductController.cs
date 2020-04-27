using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Threading.Tasks;
using VG_AspNetCore_Data.Models;
using VG_AspNetCore_Web.Services;

namespace VG_AspNetCore_Web.Api
{
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly IProductsService _productsService;

        public ProductController(IProductsService productsService)
        {
            _productsService = productsService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Get(bool includeCategory = false, bool includeSupplier = false, bool includeOrderDetails = false)
        {
            var products = await _productsService.GetAllAsync(includeCategory, includeSupplier, includeOrderDetails);
            return Ok(products);
        }


        [HttpGet("{id}", Name = "ProductGet")]
        public async Task<IActionResult> Get(int id, bool includeCategory = false, bool includeSupplier = false, bool includeOrderDetails = false)
        {
            try
            {
                Products products = await _productsService.GetAsync(id, includeCategory, includeSupplier, includeOrderDetails);
                if (products == null)
                {
                    return NotFound($"Product witn id-{id} was not found");
                }
                return Ok(products);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
            }
            return BadRequest();
        }
    }
}
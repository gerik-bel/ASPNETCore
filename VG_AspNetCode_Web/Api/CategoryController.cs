using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Threading.Tasks;
using VG_AspNetCore_Data.Models;
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
        public async Task<IActionResult> Get(bool includeProducts = false)
        {
            var categories = await _categoriesService.GetAllAsync(includeProducts);
            return Ok(categories);
        }

        [HttpGet("{id}", Name = "CategoryGet")]
        public async Task<IActionResult> Get(int id, bool includeProducts = false)
        {
            try
            {
                Categories category = await _categoriesService.GetAsync(id, includeProducts);
                if (category == null)
                {
                    return NotFound($"Category witn id-{id} was not found");
                }
                return Ok(category);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
            }
            return BadRequest();
        }
    }
}
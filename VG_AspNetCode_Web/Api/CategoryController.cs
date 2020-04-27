using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;
using VG_AspNetCore_Data.Models;
using VG_AspNetCore_Web.ActionFilters;
using VG_AspNetCore_Web.Services;

namespace VG_AspNetCore_Web.Api
{
    [Route("api/[controller]")]
    [ValidateModel]
    public class CategoryController : Controller
    {
        private const string DefImageType = "image/bmp";
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

        [HttpGet("{id}/image", Name = "CategoryImageGet")]
        public async Task<IActionResult> Get(int id)
        {
            var image = await _categoriesService.GetImageAsync(id);
            if (image != null)
            {
                return File(image, DefImageType);
            }
            return NotFound($"Image for category witn id-{id} was not found");
        }

        [HttpPost("{id}/image", Name = "CategoryImageUpload")]
        public async Task<IActionResult> Post(int id, [FromForm]IFormFile file)
        {
            if (file != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    if (memoryStream.Length < 2097152)
                    {

                        if (await _categoriesService.UpdateImageAsync(id, memoryStream.ToArray()))
                        {
                            return Ok($"{file.FileName} successfully uploaded into category with id - {id}");
                        }
                        else
                        {
                            return NotFound($"Category witn id-{id} was not found");
                        }
                    }
                    return BadRequest("The file is too large, should be less than 2MB");
                }
            }
            return BadRequest("The file is empty");
        }
    }
}
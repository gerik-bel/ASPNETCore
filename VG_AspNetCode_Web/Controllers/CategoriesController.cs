using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using VG_AspNetCore_Web.ActionFilters;
using VG_AspNetCore_Web.Services;
using VG_AspNetCore_Web.ViewModels;

namespace VG_AspNetCore_Web.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly string _permittedExtension = ".bmp";
        private const string DefImageType = "image/bmp";
        private readonly ICategoriesService _categoriesService;

        public CategoriesController(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }

        [LogAction(true)]
        public async Task<IActionResult> Index()
        {
            var categories = await _categoriesService.GetAllAsync();
            return View(categories);
        }

        [HttpGet]
        [Route("images/{id}")]
        [Route("[controller]/[action]/{id}")]
        public async Task<IActionResult> Image(int id)
        {
            var image = await _categoriesService.GetImageAsync(id);
            return File(image, DefImageType);
        }

        [HttpGet]
        [LogAction(false)]
        public async Task<IActionResult> ImageUpload(int id)
        {
            var categories = await _categoriesService.GetAsync(id);
            var editCategoriesModel = new CategoriesEditModel
            {
                CategoryId = categories.CategoryId,
                CategoryName = categories.CategoryName,
                Description = categories.Description,
                Picture = categories.Picture
            };
            return View(editCategoriesModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ImageUpload(CategoriesEditModel editCategoriesModel)
        {
            if (ModelState.IsValid)
            {
                var ext = Path.GetExtension(editCategoriesModel.File.FileName).ToLowerInvariant();
                if (string.IsNullOrEmpty(ext) || !_permittedExtension.Equals(ext))
                {
                    ModelState.AddModelError("File", "The extension is invalid, only *.bmp is supported");
                }
                else
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await editCategoriesModel.File.CopyToAsync(memoryStream);
                        if (memoryStream.Length < 2097152)
                        {
                            await _categoriesService.UpdateImageAsync(editCategoriesModel.CategoryId,
                                memoryStream.ToArray());
                            return RedirectToAction(nameof(ImageUpload), new { id = editCategoriesModel.CategoryId });
                        }

                        ModelState.AddModelError("File", "The file is too large, should be less than 2MB");
                    }
                }
            }
            return View(editCategoriesModel);
        }
    }
}
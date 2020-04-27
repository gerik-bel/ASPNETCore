using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VG_AspNetCore_Data.Models;
using VG_AspNetCore_Web.ActionFilters;
using VG_AspNetCore_Web.Services;
using VG_AspNetCore_Web.ViewModels;

namespace VG_AspNetCore_Web.Controllers
{
    [LogAction(true)]
    public class ProductsController : Controller
    {
        private const int None = 0;
        private readonly IProductsService _productsServiceService;

        public ProductsController(IProductsService productsServiceService)
        {
            _productsServiceService = productsServiceService;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productsServiceService.GetAllAsync(includeCategory: true, includeSupplier: true);
            return View(products);
        }

        public async Task<IActionResult> Details(int id)
        {
            var model = await _productsServiceService.GetAsync(id, true, true);
            if (model == null)
            {
                return RedirectToAction(nameof(Index));
            }
            ViewData["Message"] = "Details of the product";
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var products = await _productsServiceService.GetAsync(id, true, true);
            if (products == null)
            {
                return RedirectToAction("Index", "Products");
            }
            ProductsEditModel pem = new ProductsEditModel
            {
                ProductId = products.ProductId,
                ProductName = products.ProductName,
                CategoryId = products.CategoryId,
                SupplierId = products.SupplierId,
                QuantityPerUnit = products.QuantityPerUnit,
                UnitsOnOrder = products.UnitsOnOrder,
                Discontinued = products.Discontinued,
                ReorderLevel = products.ReorderLevel,
                UnitsInStock = products.UnitsInStock,
                UnitPrice = products.UnitPrice,
                Categories = await _productsServiceService.GetCategoriesAsync(),
                Suppliers = await _productsServiceService.GetSuppliersAsync()
            };
            ViewData["Message"] = "Editing the product";
            return View(pem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductsEditModel model)
        {
            if (ModelState.IsValid)
            {
                var products = ToProducts(model);
                products = await _productsServiceService.UpdateAsync(products);
                return RedirectToAction(nameof(Details), new { id = products.ProductId });
            }
            ViewData["Message"] = "Editing the product";
            model.Categories = await _productsServiceService.GetCategoriesAsync();
            model.Suppliers = await _productsServiceService.GetSuppliersAsync();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["Message"] = "Creating the product";
            ProductsEditModel pem = new ProductsEditModel
            {
                CategoryId = null,
                SupplierId = null,
                Categories = await _productsServiceService.GetCategoriesAsync(),
                Suppliers = await _productsServiceService.GetSuppliersAsync()
            };
            return View(nameof(Edit), pem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductsEditModel model)
        {
            if (model == null)
            {
                return BadRequest(ModelState);
            }
            if (ModelState.IsValid)
            {
                var products = ToProducts(model);
                products = await _productsServiceService.AddAsync(products);
                return RedirectToAction(nameof(Details), new { id = products.ProductId });
            }
            ViewData["Message"] = "Creating the product";
            model.Categories = await _productsServiceService.GetCategoriesAsync();
            model.Suppliers = await _productsServiceService.GetSuppliersAsync();
            return View(nameof(Edit), model);
        }

        private static Products ToProducts(ProductsEditModel model)
        {
            var products = new Products
            {
                ProductId = model.ProductId,
                ProductName = model.ProductName,
                CategoryId = model.CategoryId == None ? null : model.CategoryId,
                SupplierId = model.SupplierId == None ? null : model.SupplierId,
                QuantityPerUnit = model.QuantityPerUnit,
                UnitsOnOrder = model.UnitsOnOrder,
                Discontinued = model.Discontinued,
                ReorderLevel = model.ReorderLevel,
                UnitsInStock = model.UnitsInStock,
                UnitPrice = model.UnitPrice,
            };
            return products;
        }
    }
}
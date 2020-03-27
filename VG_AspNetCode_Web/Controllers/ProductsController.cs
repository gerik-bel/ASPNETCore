using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VG_AspNetCore_Web.Models;
using VG_AspNetCore_Web.Services;
using VG_AspNetCore_Web.ViewModels;

namespace VG_AspNetCore_Web.Controllers
{
    public class ProductsController : Controller
    {
        private const int None = 0;
        private readonly SelectListItem _emptyItem = new SelectListItem("None", None.ToString(), true);
        private readonly IProductsService _productsServiceService;
        public ProductsController(IProductsService productsServiceService)
        {
            _productsServiceService = productsServiceService;
        }

        public IActionResult Index()
        {
            var products = _productsServiceService.GetAll();
            return View(products);
        }

        public IActionResult Details(int id)
        {
            var model = _productsServiceService.Get(id);
            if (model == null)
            {
                return RedirectToAction(nameof(Index));
            }
            ViewData["Message"] = "Details of the product";
            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var products = _productsServiceService.Get(id);
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
                Categories = GetCategories(),
                Suppliers = GetSuppliers()
            };
            ViewData["Message"] = "Editing the product";
            return View(pem);
        }

        private List<SelectListItem> GetSuppliers()
        {
            var suppliers = _productsServiceService.GetAllSuppliers()
                .Select(p => new SelectListItem(p.CompanyName, p.SupplierId.ToString())).ToList();
            suppliers.Add(_emptyItem);
            return suppliers;
        }

        private List<SelectListItem> GetCategories()
        {
            var categories = _productsServiceService.GetAllCategories()
                .Select(p => new SelectListItem(p.CategoryName, p.CategoryId.ToString())).ToList();
            categories.Add(_emptyItem);
            return categories;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProductsEditModel model)
        {
            if (ModelState.IsValid)
            {
                var products = ToProducts(model);
                products = _productsServiceService.Update(products);
                return RedirectToAction(nameof(Details), new { id = products.ProductId });
            }
            ViewData["Message"] = "Editing the product";
            model.Categories = GetCategories();
            model.Suppliers = GetSuppliers();
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["Message"] = "Creating the product";
            ProductsEditModel pem = new ProductsEditModel
            {
                CategoryId = null,
                SupplierId = null,
                Categories = GetCategories(),
                Suppliers = GetSuppliers()
            };
            return View(nameof(Edit), pem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductsEditModel model)
        {
            if (ModelState.IsValid)
            {
                var products = ToProducts(model);
                products = _productsServiceService.Add(products);
                return RedirectToAction(nameof(Details), new { id = products.ProductId });
            }
            ViewData["Message"] = "Creating the product";
            model.Categories = GetCategories();
            model.Suppliers = GetSuppliers();
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
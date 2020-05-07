using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VG_AspNetCore_Web.Controllers;
using VG_AspNetCore_Web.Models;
using VG_AspNetCore_Web.Services;
using VG_AspNetCore_Web.ViewModels;
using Xunit;

namespace VG_AspNetCore_Web.UnitTests
{
    public class ProductsControllerTests
    {
        private const int None = 0;

        [Fact]
        public async Task Index_ReturnsAViewResult_WithAListOfProducts()
        {
            var mockService = new Mock<IProductsService>();
            mockService.Setup(p => p.GetAllAsync(true, true, false)).ReturnsAsync(GetTestProducts());
            var controller = new ProductsController(mockService.Object);
            var result = await controller.Index();
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Products>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public async Task DetailsReturnsARedirectToIndexWhenIdIsNotFound()
        {
            int testProductId = 0;
            var mockService = new Mock<IProductsService>();
            mockService.Setup(p => p.GetAsync(testProductId, true, true, false)).ReturnsAsync((Products)null);
            var controller = new ProductsController(mockService.Object);
            var result = await controller.Details(testProductId);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public async Task EditReturnsViewResultWithProductsViewModel()
        {
            int testProductId = 1;
            var mockService = new Mock<IProductsService>();
            mockService.Setup(p => p.GetAsync(testProductId, true, true, false)).ReturnsAsync(GetTestProducts().FirstOrDefault(
                    s => s.ProductId == testProductId));
            var controller = new ProductsController(mockService.Object);
            var result = await controller.Edit(testProductId);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<ProductsEditModel>(viewResult.ViewData.Model);
            Assert.Equal("test1", model.ProductName);
            Assert.True(model.Discontinued);
            Assert.Equal(testProductId, model.ProductId);
        }

        [Fact]
        public async Task Update_ReturnsEditView_withNotValidModel()
        {
            var testProductName = "owiherofwioerwueioruwoieurowieuriouweoiruwoeiruioweurweporwpoerpowuerpiuwoiwueroiuwoeiruwwoieur";
            var mockService = new Mock<IProductsService>();
            mockService.Setup(p => p.GetCategoriesAsync()).ReturnsAsync(GetTestCategories());
            mockService.Setup(p => p.GetSuppliersAsync()).ReturnsAsync(GetTestSuppliers());
            var controller = new ProductsController(mockService.Object);
            controller.ModelState.AddModelError("error", "some error");
            var result = await controller.Edit(new ProductsEditModel { ProductName = testProductName });
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<ProductsEditModel>(viewResult.ViewData.Model);
            Assert.Equal(testProductName, model.ProductName);
        }

        [Fact]
        public async Task Create_ReturnsAViewResult_WithProductsViewModel()
        {
            var mockService = new Mock<IProductsService>();
            mockService.Setup(p => p.GetCategoriesAsync()).ReturnsAsync(GetTestCategories());
            mockService.Setup(p => p.GetSuppliersAsync()).ReturnsAsync(GetTestSuppliers());
            var controller = new ProductsController(mockService.Object);
            var result = await controller.Create();
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ProductsEditModel>(viewResult.ViewData.Model);
            var category = model.Categories.FirstOrDefault(p => p.Value == "1");
            Assert.NotNull(category);
            Assert.Equal("Category1", category.Text);
        }

        [Fact]
        public async Task Create_ReturnsBadRequest_GivenInvalidModel()
        {
            var mockService = new Mock<IProductsService>();
            var controller = new ProductsController(mockService.Object);
            controller.ModelState.AddModelError("error", "some error");
            var result = await controller.Create(null);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        private List<Products> GetTestProducts()
        {
            var products = new List<Products>
            {
                new Products { ProductId = 1, ProductName = "test1", Discontinued = true},
                new Products { ProductId = 2, ProductName = "test2", Discontinued = true}
            };
            return products;
        }

        private List<SelectListItem> GetTestSuppliers()
        {
            var suppliers = new List<SelectListItem>
            {
                new SelectListItem("Supplier1", "1"),
                new SelectListItem("Supplier2", "2")
            };
            return suppliers;
        }

        private List<SelectListItem> GetTestCategories()
        {
            var categories = new List<SelectListItem>
            {
                new SelectListItem("Category1", "1"),
                new SelectListItem("Category2", "2")
            };
            return categories;
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
                UnitPrice = model.UnitPrice
            };
            return products;
        }
    }
}

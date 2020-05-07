using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VG_AspNetCore_Web.Controllers;
using VG_AspNetCore_Web.Models;
using VG_AspNetCore_Web.Services;
using Xunit;

namespace VG_AspNetCore_Web.UnitTests
{
    public class CategoriesControllerTests
    {
        [Fact]
        public async Task Index_ReturnsAViewResult_WithAListOfCategories()
        {
            var mockService = new Mock<ICategoriesService>();
            mockService.Setup(p => p.GetAllAsync(false)).ReturnsAsync(GetTestCategories());
            var controller = new CategoriesController(mockService.Object);
            var result = await controller.Index();
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Categories>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());
        }

        private List<Categories> GetTestCategories()
        {
            var categories = new List<Categories>
            {
                new Categories {CategoryId = 1, CategoryName = "test1", Description = "description fot test1"},
                new Categories {CategoryId = 2, CategoryName = "test2", Description = "description fot test2"}
            };
            return categories;
        }
    }
}
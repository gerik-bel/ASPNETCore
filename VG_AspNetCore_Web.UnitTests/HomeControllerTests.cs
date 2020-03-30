using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using VG_AspNetCore_Web.Controllers;
using VG_AspNetCore_Web.Services;
using Xunit;

namespace VG_AspNetCore_Web.UnitTests
{
    public class HomeControllerTests
    {
        [Fact]
        public async Task Index_ReturnsAViewResult()
        {
            var mockService = new Mock<IHomeService>();
            mockService.Setup(p => p.GetIndexMessage()).Returns(GetTestIndexMessage());
            var controller = new HomeController(mockService.Object);
            var result = controller.Index();
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewData.Model);
        }

        [Fact]
        public async Task About_ReturnsAViewResult()
        {
            var mockService = new Mock<IHomeService>();
            mockService.Setup(p => p.GetIndexMessage()).Returns(GetTestAboutMessage());
            var controller = new HomeController(mockService.Object);
            var result = controller.Index();
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewData.Model);
        }

        [Fact]
        public async Task Contact_ReturnsAViewResult()
        {
            var mockService = new Mock<IHomeService>();
            mockService.Setup(p => p.GetIndexMessage()).Returns(GetTestContactMessage());
            var controller = new HomeController(mockService.Object);
            var result = controller.Index();
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewData.Model);
        }

        private string GetTestIndexMessage()
        {
            return "test index message";
        }

        private string GetTestContactMessage()
        {
            return "test contact message";
        }

        private string GetTestAboutMessage()
        {
            return "test about message";
        }
    }
}
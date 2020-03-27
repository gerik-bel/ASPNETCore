using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using VG_AspNetCore_Web.Models;
using VG_AspNetCore_Web.Services;

namespace VG_AspNetCore_Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHomeService _homeService;

        public HomeController(IHomeService homeService)
        {
            _homeService = homeService;
        }

        public IActionResult Index()
        {
            ViewData["Message"] = _homeService.GetIndexMessage();
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = _homeService.GetAboutMessage();
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = _homeService.GetContactMessage();
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            var error = HttpContext.Features.Get<IExceptionHandlerFeature>().Error;
            Log.Error(error, $"Id: {requestId} Error: {error.Message}");
            return View(new ErrorViewModel { RequestId = requestId });
        }
    }
}
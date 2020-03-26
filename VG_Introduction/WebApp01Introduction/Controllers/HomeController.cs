using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Diagnostics;
using WebApp01Introduction.Models;

namespace WebApp01Introduction.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Message"] = "Welcome to Test ASP.NET Core application";
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Test ASP.NET Core application";
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "In case of any questions, feel free to contact directly";
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
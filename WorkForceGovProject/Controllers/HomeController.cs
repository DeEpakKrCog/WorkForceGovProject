using Microsoft.AspNetCore.Mvc;

namespace WorkForceGovProject.Controllers
{
    // The class name MUST match the file name: HomeController
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View(); // This renders Views/Home/Index.cshtml
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
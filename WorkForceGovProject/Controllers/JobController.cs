using Microsoft.AspNetCore.Mvc;

namespace WorkForceGovProject.Controllers
{
    public class JobController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

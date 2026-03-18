using System.Web.Mvc;
using WorkForceGov.Models;

namespace WorkForceGov.Controllers
{
    public class ProgramManagerController : Controller
    {
        public ActionResult Dashboard()
        {
            // Sample data (normally fetched from database)
            ProgramManagerDashboardViewModel model = new ProgramManagerDashboardViewModel
            {
                TotalPrograms = 7,
                TotalBudget = 1200000,
                ActiveTrainingPrograms = 5,
                PerformancePercentage = 78.5
            };

            return View(model);
        }
    }
}

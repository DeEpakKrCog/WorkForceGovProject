
using Microsoft.AspNetCore.Mvc;
using WorkForceGovProject.Models;

namespace WorkForceGovProject.Controllers
{
    public class ProgramManagerController : Controller
    {
        public IActionResult Dashboard()
        {
            ProgramManagerDashboardViewModel model =
                new ProgramManagerDashboardViewModel
                {
                    TotalPrograms = 7,
                    TotalBudget = 1200000,
                    ActiveTrainingPrograms = 5,
                    PerformancePercentage = 78
                };

            return View(model);
        }
    }
}

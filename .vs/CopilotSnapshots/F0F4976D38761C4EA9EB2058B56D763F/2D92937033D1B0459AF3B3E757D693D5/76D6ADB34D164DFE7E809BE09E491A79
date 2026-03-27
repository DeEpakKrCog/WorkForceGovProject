using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using WorkForceGovProject.Data;
using WorkForceGovProject.Models; // Ensure this is included for the Citizen model
using System.Linq;

namespace WorkForceGovProject.Controllers
{
    public class CitizenController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CitizenController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            var citizen = _context.Citizens.FirstOrDefault(c => c.UserId == userId);

            if (citizen == null)
            {
                citizen = new Citizen
                {
                    UserId = userId.Value,
                    FullName = HttpContext.Session.GetString("UserName") ?? "Citizen User",
                    Email = HttpContext.Session.GetString("UserEmail") ?? "", // ADD THIS LINE
                    ActiveApplications = 0,
                    AccountBalance = 0.00m,
                    DocumentStatus = "Pending",
                    NewJobMatches = 0
                };

                _context.Citizens.Add(citizen);
                _context.SaveChanges(); // This won't crash now!
            }

            return View(citizen);
        }
    }
}
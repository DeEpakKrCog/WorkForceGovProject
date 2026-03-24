using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using WorkForceGovProject.Data;
using WorkForceGovProject.Models;
using System.Linq;

namespace WorkForceGovProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context) => _context = context;

        [HttpGet] public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);

            if (user != null)
            {
                // Store data for the Layout and Profile logic
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("UserName", user.FullName);
                HttpContext.Session.SetString("UserRole", user.Role);

                if (user.Role == "Citizen") return RedirectToAction("Dashboard", "Citizen");
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Invalid email or password.";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}
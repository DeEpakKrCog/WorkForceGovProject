using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http; // Required for Sessions
using WorkForceGovProject.Data;
using WorkForceGovProject.Models;
using System.Linq;

namespace WorkForceGovProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                // Basic check to prevent duplicate emails
                if (_context.Users.Any(u => u.Email == user.Email))
                {
                    ModelState.AddModelError("Email", "This email is already registered.");
                    return View(user);
                }

                _context.Users.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Login");
            }
            return View(user);
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);

            if (user != null)
            {
                // 1. Store User Info in Session (Optional but recommended)
                HttpContext.Session.SetString("UserName", user.FullName);
                HttpContext.Session.SetString("UserRole", user.Role);
                HttpContext.Session.SetInt32("UserId", user.Id);

                // 2. Role-Based Redirection Logic
                if (user.Role == "Citizen")
                {
                    return RedirectToAction("Dashboard", "Citizen");
                }
                else if (user.Role == "Admin")
                {
                    // return RedirectToAction("AdminPanel", "Admin"); 
                    return RedirectToAction("Index", "Home");
                }

                // Default fallback
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Invalid email or password.";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Clears login data
            return RedirectToAction("Index", "Home");
        }
    }
}
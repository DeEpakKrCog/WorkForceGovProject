using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using WorkForceGovProject.Models;
using WorkForceGovProject.Services;

namespace WorkForceGovProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly ICitizenService _citizenService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAccountService accountService, ICitizenService citizenService, ILogger<AccountController> logger)
        {
            _accountService = accountService;
            _citizenService = citizenService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            var (success, message, user) = await _accountService.LoginAsync(email, password);

            if (success && user != null)
            {
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("UserName", user.FullName ?? "User");
                HttpContext.Session.SetString("UserRole", user.Role);
                HttpContext.Session.SetString("UserEmail", user.Email);

                TempData["SuccessMessage"] = $"Welcome back, {user.FullName}!";

                switch (user.Role)
                {
                    case "Citizen":
                        var citizen = await _citizenService.GetCitizenByUserIdAsync(user.Id);
                        if (citizen == null)
                        {
                            await _citizenService.CreateCitizenProfileAsync(user.Id, user.FullName, user.Email);
                        }
                        return RedirectToAction("Dashboard", "Citizen");

                    case "Employer":
                        return RedirectToAction("Dashboard", "Employer");

                    default:
                        return RedirectToAction("Index", "Home");
                }
            }

            TempData["ErrorMessage"] = message;
            return View();
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User model)
        {
            if (ModelState.IsValid)
            {
                var (success, message) = await _accountService.RegisterAsync(model);

                if (success)
                {
                    TempData["SuccessMessage"] = "Registration successful! You can now login.";
                    return RedirectToAction("Login");
                }

                TempData["ErrorMessage"] = message;
            }
            else
            {
                TempData["ErrorMessage"] = "Please fill in all required fields correctly.";
            }

            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["SuccessMessage"] = "You have been logged out.";
            return RedirectToAction("Login");
        }
    }
}
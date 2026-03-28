using Microsoft.AspNetCore.Mvc;
using WorkForceGovProject.Data;
using WorkForceGovProject.Models;
using WorkForceGovProject.Services;

namespace WorkForceGovProject.Controllers
{
    [Route("Citizen")]
    public class CitizenController : Controller
    {
        private readonly IApplicationService _applicationService;
        private readonly ICitizenService _citizenService;
        private readonly IDocumentService _documentService;
        private readonly IJobService _jobService;
        private readonly IBenefitService _benefitService;
        private readonly INotificationService _notificationService;
        private readonly ApplicationDbContext _context;

        public CitizenController(
            IApplicationService applicationService,
            ICitizenService citizenService,
            IDocumentService documentService,
            IJobService jobService,
            IBenefitService benefitService,
            INotificationService notificationService,
            ApplicationDbContext context)
        {
            _applicationService = applicationService;
            _citizenService = citizenService;
            _documentService = documentService;
            _jobService = jobService;
            _benefitService = benefitService;
            _notificationService = notificationService;
            _context = context;
        }

        // Helper method with improved Null Safety
        private async Task<Citizen?> GetLoggedInCitizenAsync()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            // If Session is empty, we can't find a citizen
            if (!userId.HasValue) return null;

            // This returns null if the user exists but hasn't filled out a profile
            return await _citizenService.GetCitizenByUserIdAsync(userId.Value);
        }

        [Route("Dashboard")]
        public async Task<IActionResult> Dashboard()
        {
            var citizen = await GetLoggedInCitizenAsync();

            // FIX: If no profile exists, redirect to profile creation or login
            if (citizen == null)
            {
                TempData["ErrorMessage"] = "Please complete your profile registration.";
                return RedirectToAction("CreateCitizen");
            }

            // Safe to access .Id now
            await _citizenService.UpdateDashboardStatsAsync(citizen.Id);
            var updatedCitizen = await _citizenService.GetCitizenByIdAsync(citizen.Id);

            return View(updatedCitizen);
        }

        [Route("Profile")]
        public async Task<IActionResult> Profile()
        {
            var citizen = await GetLoggedInCitizenAsync();
            if (citizen == null) return RedirectToAction("CreateCitizen");

            return View(citizen);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(Citizen model)
        {
            var (success, message) = await _citizenService.UpdatePersonalInfoAsync(
                model.Id,
                model.FullName ?? string.Empty,
                model.DOB,
                model.Gender ?? string.Empty,
                model.Address ?? string.Empty,
                model.PhoneNumber ?? string.Empty);

            if (success)
                TempData["SuccessMessage"] = "Profile updated successfully!";
            else
                TempData["ErrorMessage"] = message;

            return RedirectToAction("Profile");
        }

        [Route("Documents")]
        public async Task<IActionResult> Documents()
        {
            var citizen = await GetLoggedInCitizenAsync();
            if (citizen == null) return RedirectToAction("CreateCitizen");

            var documents = await _documentService.GetCitizenDocumentsAsync(citizen.Id);
            return View(documents);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadDocument(IFormFile file, string documentType)
        {
            var citizen = await GetLoggedInCitizenAsync();
            if (citizen == null) return RedirectToAction("Login", "Account");

            var (success, message, _) = await _documentService.UploadDocumentAsync(
                citizen.Id, documentType, file);

            if (success) TempData["SuccessMessage"] = message;
            else TempData["ErrorMessage"] = message;

            return RedirectToAction("Documents");
        }

        [Route("JobSearch")]
        public async Task<IActionResult> JobSearch(string location = "", string category = "")
        {
            var citizen = await GetLoggedInCitizenAsync();
            if (citizen == null) return RedirectToAction("CreateCitizen");

            var jobs = await _jobService.SearchJobsAsync(location, category);
            return View(jobs);
        }

        [Route("Benefits")]
        public async Task<IActionResult> Benefits()
        {
            var citizen = await GetLoggedInCitizenAsync();
            if (citizen == null) return RedirectToAction("CreateCitizen");

            var benefits = await _benefitService.GetCitizenBenefitsAsync(citizen.Id);
            return View(benefits);
        }

        [Route("Notifications")]
        public async Task<IActionResult> Notifications()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue) return RedirectToAction("Login", "Account");

            var notifications = await _notificationService.GetUserNotificationsAsync(userId.Value);
            return View(notifications);
        }

        [HttpGet]
        [Route("CreateCitizen")]
        public IActionResult CreateCitizen()
        {
            return View();
        }

        [HttpPost]
        [Route("CreateCitizen")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCitizen(Citizen citizen)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue) return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                citizen.UserId = userId.Value; // Link it to the logged-in user
                citizen.ActiveApplications = 0;
                citizen.AccountBalance = 0.00m;
                citizen.DocumentStatus = "Pending";

                _context.Citizens.Add(citizen);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Profile created!";
                return RedirectToAction("Dashboard");
            }

            return View(citizen);
        }
    }
}
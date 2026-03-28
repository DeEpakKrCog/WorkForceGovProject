using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkForceGovProject.Data;
using WorkForceGovProject.Interfaces;
using WorkForceGovProject.Models;

namespace WorkForceGovProject.Controllers
{
    public class EmployerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmployerRepository _employerRepository;
        private readonly IJobOpeningRepository _jobOpeningRepository;
        private readonly IApplicationRepository _applicationRepository;
        private readonly IEmployerDocumentRepository _employerDocumentRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly ICitizenRepository _citizenRepository;

        public EmployerController(
            ApplicationDbContext context,
            IEmployerRepository employerRepository,
            IJobOpeningRepository jobOpeningRepository,
            IApplicationRepository applicationRepository,
            IEmployerDocumentRepository employerDocumentRepository,
            INotificationRepository notificationRepository,
            ICitizenRepository citizenRepository)
        {
            _context = context;
            _employerRepository = employerRepository;
            _jobOpeningRepository = jobOpeningRepository;
            _applicationRepository = applicationRepository;
            _employerDocumentRepository = employerDocumentRepository;
            _notificationRepository = notificationRepository;
            _citizenRepository = citizenRepository;
        }

        // Helper method to get current employer
        private async Task<Employer> GetCurrentEmployerAsync()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return null;

            return await _employerRepository.GetEmployerByUserIdAsync(userId.Value);
        }

        #region Registration

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Employer model, string email, string password)
        {
            try
            {
                // Validate email and password are provided
                if (string.IsNullOrWhiteSpace(email))
                {
                    ModelState.AddModelError("email", "Email is required.");
                }
                if (string.IsNullOrWhiteSpace(password))
                {
                    ModelState.AddModelError("password", "Password is required.");
                }

                if (!ModelState.IsValid)
                {
                    // Show validation errors
                    TempData["ErrorMessage"] = "Please fill all required fields correctly.";
                    return View(model);
                }

                // Check if email already exists
                if (await _context.Users.AnyAsync(u => u.Email == email))
                {
                    ModelState.AddModelError("email", "This email is already registered.");
                    TempData["ErrorMessage"] = "This email is already registered. Please use a different email.";
                    return View(model);
                }

                // Create User account
                var user = new User
                {
                    FullName = model.CompanyName,
                    Email = email,
                    Password = password,
                    Role = "Employer"
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // Create Employer profile
                model.UserId = user.Id;
                model.RegistrationDate = DateTime.Now;
                model.Status = "Pending";

                await _employerRepository.CreateEmployerAsync(model);

                TempData["SuccessMessage"] = "Registration successful! Please wait for account verification.";
                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                // Log the error and show user-friendly message
                TempData["ErrorMessage"] = $"Registration failed: {ex.Message}";
                return View(model);
            }
        }

        #endregion

        #region Dashboard

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                TempData["ErrorMessage"] = "Please login first.";
                return RedirectToAction("Login", "Account");
            }

            var employer = await GetCurrentEmployerAsync();
            if (employer == null)
            {
                // Debug: Check if user exists
                var user = await _context.Users.FindAsync(userId.Value);
                if (user == null)
                {
                    TempData["ErrorMessage"] = $"User account not found (ID: {userId.Value}). Please contact support.";
                    HttpContext.Session.Clear();
                    return RedirectToAction("Login", "Account");
                }

                // Check if employer profile exists
                var employerProfile = await _employerRepository.GetEmployerByUserIdAsync(userId.Value);
                if (employerProfile == null)
                {
                    TempData["ErrorMessage"] = $"Employer profile not found for user '{user.Email}'. Please complete registration or contact support.";
                    return RedirectToAction("Register", "Employer");
                }

                TempData["ErrorMessage"] = "Employer profile not found.";
                return RedirectToAction("Index", "Home");
            }

            // Get dashboard statistics
            var totalJobPostings = await _jobOpeningRepository.GetJobOpeningsByEmployerIdAsync(employer.Id);
            var allApplications = await _applicationRepository.GetApplicationsByEmployerIdAsync(employer.Id);

            ViewBag.TotalJobPostings = totalJobPostings.Count();
            ViewBag.ActiveJobPostings = await _jobOpeningRepository.GetActiveJobsCountByEmployerAsync(employer.Id);
            ViewBag.TotalApplicants = allApplications.Count();
            ViewBag.PendingApplications = await _applicationRepository.GetPendingApplicationsCountAsync(employer.Id);
            ViewBag.ShortlistedCandidates = allApplications.Count(a => a.Status == "Shortlisted");
            ViewBag.HiredCandidates = allApplications.Count(a => a.Status == "Approved");
            ViewBag.UnreadNotifications = await _notificationRepository.GetUnreadCountAsync(userId.Value);

            return View(employer);
        }

        #endregion

        #region Profile

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var employer = await GetCurrentEmployerAsync();
            if (employer == null)
            {
                return RedirectToAction("Login", "Account");
            }

            return View(employer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(Employer model)
        {
            var employer = await GetCurrentEmployerAsync();
            if (employer == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                employer.CompanyName = model.CompanyName;
                employer.Industry = model.Industry;
                employer.Address = model.Address;
                employer.ContactInfo = model.ContactInfo;

                await _employerRepository.UpdateEmployerAsync(employer);

                TempData["SuccessMessage"] = "Profile updated successfully!";
                return RedirectToAction("Profile");
            }

            return View(employer);
        }

        #endregion

        #region Job Posting

        [HttpGet]
        public async Task<IActionResult> CreateJob()
        {
            var employer = await GetCurrentEmployerAsync();
            if (employer == null)
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateJob(JobOpening model)
        {
            var employer = await GetCurrentEmployerAsync();
            if (employer == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                model.EmployerId = employer.Id;
                model.PostedDate = DateTime.Now;
                model.Status = "Open";

                await _jobOpeningRepository.CreateJobOpeningAsync(model);

                TempData["SuccessMessage"] = "Job posted successfully!";
                return RedirectToAction("ManageJobs");
            }

            return View(model);
        }

        #endregion

        #region Manage Job Listings

        [HttpGet]
        public async Task<IActionResult> ManageJobs()
        {
            var employer = await GetCurrentEmployerAsync();
            if (employer == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var jobs = await _jobOpeningRepository.GetJobOpeningsByEmployerIdAsync(employer.Id);
            return View(jobs);
        }

        [HttpGet]
        public async Task<IActionResult> EditJob(int id)
        {
            var employer = await GetCurrentEmployerAsync();
            if (employer == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var job = await _jobOpeningRepository.GetJobOpeningByIdAsync(id);
            if (job == null || job.EmployerId != employer.Id)
            {
                TempData["ErrorMessage"] = "Job not found.";
                return RedirectToAction("ManageJobs");
            }

            return View(job);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditJob(JobOpening model)
        {
            var employer = await GetCurrentEmployerAsync();
            if (employer == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                var job = await _jobOpeningRepository.GetJobOpeningByIdAsync(model.Id);
                if (job != null && job.EmployerId == employer.Id)
                {
                    job.Title = model.Title;
                    job.Description = model.Description;
                    job.Location = model.Location;
                    job.Salary = model.Salary;
                    job.Requirements = model.Requirements;
                    job.ApplicationDeadline = model.ApplicationDeadline;
                    job.Status = model.Status;

                    await _jobOpeningRepository.UpdateJobOpeningAsync(job);

                    TempData["SuccessMessage"] = "Job updated successfully!";
                    return RedirectToAction("ManageJobs");
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteJob(int id)
        {
            var employer = await GetCurrentEmployerAsync();
            if (employer == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var job = await _jobOpeningRepository.GetJobOpeningByIdAsync(id);
            if (job != null && job.EmployerId == employer.Id)
            {
                await _jobOpeningRepository.DeleteJobOpeningAsync(id);
                TempData["SuccessMessage"] = "Job deleted successfully!";
            }

            return RedirectToAction("ManageJobs");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CloseJob(int id)
        {
            var employer = await GetCurrentEmployerAsync();
            if (employer == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var job = await _jobOpeningRepository.GetJobOpeningByIdAsync(id);
            if (job != null && job.EmployerId == employer.Id)
            {
                await _jobOpeningRepository.CloseJobOpeningAsync(id);
                TempData["SuccessMessage"] = "Job closed successfully!";
            }

            return RedirectToAction("ManageJobs");
        }

        #endregion

        #region Applications

        [HttpGet]
        public async Task<IActionResult> Applications(int? jobId)
        {
            var employer = await GetCurrentEmployerAsync();
            if (employer == null)
            {
                return RedirectToAction("Login", "Account");
            }

            IEnumerable<Application> applications;

            if (jobId.HasValue)
            {
                // Get applications for specific job
                var job = await _jobOpeningRepository.GetJobOpeningByIdAsync(jobId.Value);
                if (job == null || job.EmployerId != employer.Id)
                {
                    TempData["ErrorMessage"] = "Job not found.";
                    return RedirectToAction("Dashboard");
                }

                applications = await _applicationRepository.GetApplicationsByJobIdAsync(jobId.Value);
                ViewBag.JobTitle = job.Title;
            }
            else
            {
                // Get all applications for this employer
                applications = await _applicationRepository.GetApplicationsByEmployerIdAsync(employer.Id);
            }

            // Get job and citizen details for each application
            var applicationList = new List<dynamic>();
            foreach (var app in applications)
            {
                var job = await _jobOpeningRepository.GetJobOpeningByIdAsync(app.JobId);
                var citizen = await _citizenRepository.GetCitizenByIdAsync(app.CitizenId);

                applicationList.Add(new
                {
                    Application = app,
                    JobTitle = job?.Title,
                    CitizenName = citizen?.FullName,
                    CitizenEmail = citizen?.Email
                });
            }

            ViewBag.ApplicationDetails = applicationList;
            return View(applications);
        }

        [HttpGet]
        public async Task<IActionResult> ApplicationDetails(int id)
        {
            var employer = await GetCurrentEmployerAsync();
            if (employer == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var application = await _applicationRepository.GetApplicationByIdAsync(id);
            if (application == null)
            {
                TempData["ErrorMessage"] = "Application not found.";
                return RedirectToAction("Applications");
            }

            // Verify this application belongs to employer's job
            var job = await _jobOpeningRepository.GetJobOpeningByIdAsync(application.JobId);
            if (job == null || job.EmployerId != employer.Id)
            {
                TempData["ErrorMessage"] = "Unauthorized access.";
                return RedirectToAction("Applications");
            }

            var citizen = await _citizenRepository.GetCitizenByIdAsync(application.CitizenId);

            ViewBag.Job = job;
            ViewBag.Citizen = citizen;

            return View(application);
        }

        #endregion

        #region Candidate Selection

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ShortlistApplication(int id)
        {
            var employer = await GetCurrentEmployerAsync();
            if (employer == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var application = await _applicationRepository.GetApplicationByIdAsync(id);
            if (application != null)
            {
                var job = await _jobOpeningRepository.GetJobOpeningByIdAsync(application.JobId);
                if (job != null && job.EmployerId == employer.Id)
                {
                    await _applicationRepository.ShortlistApplicationAsync(id);

                    // Create notification for citizen
                    var citizen = await _citizenRepository.GetCitizenByIdAsync(application.CitizenId);
                    if (citizen != null)
                    {
                        await _notificationRepository.CreateNotificationAsync(new Notification
                        {
                            UserId = citizen.UserId,
                            EntityId = application.Id,
                            Message = $"Your application for {job.Title} has been shortlisted!",
                            Category = "Application Update",
                            Status = "Unread",
                            CreatedDate = DateTime.Now
                        });
                    }

                    TempData["SuccessMessage"] = "Application shortlisted successfully!";
                }
            }

            return RedirectToAction("Applications");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveApplication(int id)
        {
            var employer = await GetCurrentEmployerAsync();
            if (employer == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var application = await _applicationRepository.GetApplicationByIdAsync(id);
            if (application != null)
            {
                var job = await _jobOpeningRepository.GetJobOpeningByIdAsync(application.JobId);
                if (job != null && job.EmployerId == employer.Id)
                {
                    await _applicationRepository.ApproveApplicationAsync(id);

                    // Create notification for citizen
                    var citizen = await _citizenRepository.GetCitizenByIdAsync(application.CitizenId);
                    if (citizen != null)
                    {
                        await _notificationRepository.CreateNotificationAsync(new Notification
                        {
                            UserId = citizen.UserId,
                            EntityId = application.Id,
                            Message = $"Congratulations! You have been hired for {job.Title}!",
                            Category = "Hiring",
                            Status = "Unread",
                            CreatedDate = DateTime.Now
                        });
                    }

                    TempData["SuccessMessage"] = "Candidate hired successfully!";
                }
            }

            return RedirectToAction("Applications");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectApplication(int id)
        {
            var employer = await GetCurrentEmployerAsync();
            if (employer == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var application = await _applicationRepository.GetApplicationByIdAsync(id);
            if (application != null)
            {
                var job = await _jobOpeningRepository.GetJobOpeningByIdAsync(application.JobId);
                if (job != null && job.EmployerId == employer.Id)
                {
                    await _applicationRepository.RejectApplicationAsync(id);

                    // Create notification for citizen
                    var citizen = await _citizenRepository.GetCitizenByIdAsync(application.CitizenId);
                    if (citizen != null)
                    {
                        await _notificationRepository.CreateNotificationAsync(new Notification
                        {
                            UserId = citizen.UserId,
                            EntityId = application.Id,
                            Message = $"Your application for {job.Title} has been reviewed.",
                            Category = "Application Update",
                            Status = "Unread",
                            CreatedDate = DateTime.Now
                        });
                    }

                    TempData["SuccessMessage"] = "Application rejected.";
                }
            }

            return RedirectToAction("Applications");
        }

        [HttpGet]
        public async Task<IActionResult> ShortlistedCandidates()
        {
            var employer = await GetCurrentEmployerAsync();
            if (employer == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var applications = await _applicationRepository.GetShortlistedApplicationsAsync(employer.Id);

            // Get job and citizen details for each application
            var applicationList = new List<dynamic>();
            foreach (var app in applications)
            {
                var job = await _jobOpeningRepository.GetJobOpeningByIdAsync(app.JobId);
                var citizen = await _citizenRepository.GetCitizenByIdAsync(app.CitizenId);

                applicationList.Add(new
                {
                    Application = app,
                    JobTitle = job?.Title,
                    CitizenName = citizen?.FullName,
                    CitizenEmail = citizen?.Email
                });
            }

            ViewBag.ApplicationDetails = applicationList;
            return View(applications);
        }

        #endregion

        #region Notifications

        [HttpGet]
        public async Task<IActionResult> Notifications()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var notifications = await _notificationRepository.GetNotificationsByUserIdAsync(userId.Value);
            return View(notifications);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkNotificationAsRead(int id)
        {
            await _notificationRepository.MarkAsReadAsync(id);
            return RedirectToAction("Notifications");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAllNotificationsAsRead()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId.HasValue)
            {
                await _notificationRepository.MarkAllAsReadAsync(userId.Value);
            }
            return RedirectToAction("Notifications");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            await _notificationRepository.DeleteNotificationAsync(id);
            TempData["SuccessMessage"] = "Notification deleted successfully!";
            return RedirectToAction("Notifications");
        }

        #endregion

        #region Document Management

        [HttpGet]
        public async Task<IActionResult> Documents()
        {
            var employer = await GetCurrentEmployerAsync();
            if (employer == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var documents = await _employerDocumentRepository.GetDocumentsByEmployerIdAsync(employer.Id);
            return View(documents);
        }

        [HttpGet]
        public async Task<IActionResult> UploadDocument()
        {
            var employer = await GetCurrentEmployerAsync();
            if (employer == null)
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadDocument(EmployerDocument model, IFormFile documentFile)
        {
            var employer = await GetCurrentEmployerAsync();
            if (employer == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid && documentFile != null)
            {
                // Save file to server
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "documents");
                Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = $"{Guid.NewGuid()}_{documentFile.FileName}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await documentFile.CopyToAsync(fileStream);
                }

                model.EmployerId = employer.Id;
                model.FileURL = $"/uploads/documents/{uniqueFileName}";
                model.UploadedDate = DateTime.Now;
                model.VerificationStatus = "Pending";

                await _employerDocumentRepository.UploadDocumentAsync(model);

                TempData["SuccessMessage"] = "Document uploaded successfully! Waiting for verification.";
                return RedirectToAction("Documents");
            }

            return View(model);
        }

        #endregion
    }
}

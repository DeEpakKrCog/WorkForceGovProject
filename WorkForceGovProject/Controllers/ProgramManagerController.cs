using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WorkForceGovProject.Models;
using WorkForceGovProject.Repositories;

namespace WorkForceGovProject.Controllers
{
    public class ProgramManagerController : Controller
    {
        private readonly IJobRepository _jobRepo;
        private readonly ICitizenRepository _citizenRepo;
        private readonly INotificationRepository _notificationRepo;
        // Since Labor branch adds these, ensure these Repositories exist in your project
        private readonly IRepository<EmploymentProgram> _programRepo;
        private readonly IRepository<Benefit> _benefitRepo;

        public ProgramManagerController(
            IJobRepository jobRepo,
            ICitizenRepository citizenRepo,
            INotificationRepository notificationRepo,
            IRepository<EmploymentProgram> programRepo,
            IRepository<Benefit> benefitRepo)
        {
            _jobRepo = jobRepo;
            _citizenRepo = citizenRepo;
            _notificationRepo = notificationRepo;
            _programRepo = programRepo;
            _benefitRepo = benefitRepo;
        }

        // GET: ProgramManager/Dashboard
        public async Task<IActionResult> Dashboard()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            // Using the Generic Repository methods
            var allPrograms = await _programRepo.GetAllAsync();
            var allBenefits = await _benefitRepo.GetAllAsync();

            ViewBag.TotalPrograms = allPrograms.Count();
            ViewBag.ActivePrograms = allPrograms.Count(p => p.Status == "Active");
            ViewBag.TotalBudget = allPrograms.Sum(p => p.Budget);
            ViewBag.TotalBenefits = allBenefits.Sum(b => b.Amount);

            return View();
        }

        public async Task<IActionResult> ProgramManagement()
        {
            var programs = await _programRepo.GetAllAsync();
            return View(programs);
        }

        [HttpGet]
        public IActionResult CreateProgram() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProgram(EmploymentProgram program)
        {
            if (ModelState.IsValid)
            {
                await _programRepo.AddAsync(program);
                // Note: If you don't have UnitOfWork, your Repository needs a SaveChanges method 
                // or you need to inject ApplicationDbContext to save.
                return RedirectToAction("ProgramManagement");
            }
            return View(program);
        }

        // GET: Benefit Management
        public async Task<IActionResult> BenefitManagement()
        {
            var benefits = await _benefitRepo.GetAllAsync();
            return View(benefits);
        }

        // GET: Create Benefit
        public async Task<IActionResult> CreateBenefit()
        {
            var programs = await _programRepo.GetAllAsync();
            var citizens = await _citizenRepo.GetAllAsync();

            ViewBag.Programs = new SelectList(programs, "ProgramID", "Title");
            ViewBag.Citizens = new SelectList(citizens, "Id", "FullName");
            return View();
        }
    }
}
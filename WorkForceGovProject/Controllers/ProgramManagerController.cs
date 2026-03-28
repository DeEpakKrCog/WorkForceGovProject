using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkForceGovProject.Data;
using WorkForceGovProject.Models;

namespace WorkForceGovProject.Controllers
{
    [Route("ProgramManager")]
    public class ProgramManagerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProgramManagerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. Program Management (The List View)
        [HttpGet("ProgramManagement")]
        public async Task<IActionResult> ProgramManagement()
        {
            var programs = await _context.EmploymentPrograms.ToListAsync();
            return View(programs);
        }

        // 2. Performance Tracking (The Dashboard View)
        [HttpGet("PerformanceTracking")]
        public async Task<IActionResult> PerformanceTracking()
        {
            var programs = await _context.EmploymentPrograms
                .Include(p => p.Trainings)
                .Include(p => p.Benefits)
                    .ThenInclude(b => b.Citizen)
                .ToListAsync();

            // Populate ViewBags for summary cards
            ViewBag.TotalBeneficiaries = programs.SelectMany(p => p.Benefits ?? new List<Benefit>())
                                                 .Select(b => b.CitizenID).Distinct().Count();
            ViewBag.TotalBenefitsDistributed = programs.Sum(p => p.Benefits?.Count ?? 0);
            ViewBag.TotalAmountPaid = programs.Sum(p => p.Benefits?.Sum(b => b.Amount) ?? 0);

            // Recent Beneficiaries for Line 109
            ViewBag.RecentBeneficiaries = await _context.Benefits
                .Include(b => b.Citizen)
                .Include(b => b.EmploymentProgram)
                .OrderByDescending(b => b.Date)
                .Take(5)
                .ToListAsync();

            return View(programs);
        }

        // 3. Delete Program - GET (Shows confirmation page)
        // This matches Line 67 in your ProgramManagement View
        [HttpGet("DeleteProgram/{id}")]
        public async Task<IActionResult> DeleteProgram(int id)
        {
            var program = await _context.EmploymentPrograms
                .Include(p => p.Trainings)
                .Include(p => p.Benefits)
                .FirstOrDefaultAsync(m => m.ProgramID == id);

            if (program == null) return NotFound();

            return View("Delete", program);
        }

        // 4. Delete Program - POST (Actual Deletion)
        [HttpPost("ConfirmDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(int ProgramID)
        {
            var program = await _context.EmploymentPrograms.FindAsync(ProgramID);
            if (program != null)
            {
                _context.EmploymentPrograms.Remove(program);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Program deleted successfully.";
            }
            return RedirectToAction(nameof(ProgramManagement));
        }
    }
}
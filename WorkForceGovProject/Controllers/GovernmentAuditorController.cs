using Microsoft.AspNetCore.Mvc;
using WorkForceGovProject.Services;

namespace WorkForceGovProject.Controllers
{
    [Route("GovernmentAuditor")]
    public class GovernmentAuditorController : Controller
    {
        private readonly IAuditService _auditService;
        private readonly IComplianceService _complianceService;
        private readonly IReportingService _reportingService;
        private readonly ILogger<GovernmentAuditorController> _logger;

        public GovernmentAuditorController(
            IAuditService auditService,
            IComplianceService complianceService,
            IReportingService reportingService,
            ILogger<GovernmentAuditorController> logger)
        {
            _auditService = auditService;
            _complianceService = complianceService;
            _reportingService = reportingService;
            _logger = logger;
        }

        [Route("Dashboard")]
        public async Task<IActionResult> Dashboard()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            var metrics = await _reportingService.GetDashboardMetricsAsync();
            var stats = await _auditService.GetAuditStatisticsAsync();

            ViewBag.Metrics = metrics;
            ViewBag.Stats = stats;

            return View();
        }

        [Route("ComplianceMonitoring")]
        public async Task<IActionResult> ComplianceMonitoring()
        {
            var records = await _complianceService.GetAllComplaintsAsync();
            return View(records);
        }

        [Route("WorkforcePrograms")]
        public async Task<IActionResult> WorkforcePrograms()
        {
            var metrics = await _reportingService.GetDashboardMetricsAsync();
            return View(metrics);
        }

        [Route("ProgarmReview")]
        public async Task<IActionResult> ProgramReview()
        {
            var report = await _reportingService.GenerateProgramReportAsync();
            return View(report);
        }

        [Route("AuditReports")]
        public async Task<IActionResult> AuditReports()
        {
            var audits = await _auditService.GetAllAuditsAsync();
            return View(audits);
        }

        [Route("CreateAudit")]
        public IActionResult CreateAudit()
        {
            return View();
        }

        [HttpPost]
        [Route("CreateAudit")]
        public async Task<IActionResult> CreateAuditPost(string scope, string findings)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            var (success, message, audit) = await _auditService.CreateAuditAsync(userId.Value, scope, findings);
            if (success)
                TempData["SuccessMessage"] = message;
            else
                TempData["ErrorMessage"] = message;

            return RedirectToAction("AuditReports");
        }

        [Route("Notifications")]
        public IActionResult Notifications()
        {
            return View();
        }

        [Route("AlertsAndNotifications")]
        public async Task<IActionResult> AlertsAndNotifications()
        {
            var metrics = await _reportingService.GetDashboardMetricsAsync();
            ViewBag.Metrics = metrics;
            return View();
        }

        [Route("ApplicationAnalytics")]
        public async Task<IActionResult> ApplicationAnalytics()
        {
            var report = await _reportingService.GenerateApplicationReportAsync();
            return View(report);
        }

        [Route("EmployerAnalytics")]
        public async Task<IActionResult> EmployerAnalytics()
        {
            var report = await _reportingService.GenerateComplianceReportAsync("Employer");
            return View(report);
        }
    }
}

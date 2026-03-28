using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using WorkForceGovProject.Data;
using WorkForceGovProject.Models;
using WorkForceGovProject.Models.ViewModels;

namespace WorkForceGovProject.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Dashboard
        public async Task<IActionResult> Dashboard()
        {
            try
            {
                var dashboardData = new AdminDashboardViewModel
                {
                    TotalUsers = await _context.Users.CountAsync(),
                    // Note: If this line crashes, you MUST run 'Update-Database' in Package Manager Console
                    ActiveUsers = await _context.Users.CountAsync(u => u.Status == "Active"),
                    InactiveUsers = await _context.Users.CountAsync(u => u.Status == "Inactive"),
                    TotalRoles = await _context.Roles.CountAsync(),

                    RecentLogs = await _context.SystemLogs
                        .OrderByDescending(l => l.Timestamp)
                        .Take(10)
                        .ToListAsync(),

                    RecentUsers = await _context.Users
                        .OrderByDescending(u => u.CreatedAt)
                        .Take(5)
                        .ToListAsync()
                };

                return View(dashboardData);
            }
            catch (Exception ex)
            {
                // If the database is missing columns, show a friendly error or empty dashboard
                ViewBag.ErrorMessage = "Database Schema Mismatch: " + ex.Message;
                return View(new AdminDashboardViewModel());
            }
        }

        // GET: Admin/ManageUsers
        public async Task<IActionResult> ManageUsers(string status = "", string searchTerm = "")
        {
            try
            {
                var usersQuery = _context.Users.Include(u => u.RoleNavigation).AsQueryable();

                if (!string.IsNullOrEmpty(status) && status != "All")
                {
                    usersQuery = usersQuery.Where(u => u.Status == status);
                }

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    usersQuery = usersQuery.Where(u =>
                        u.FullName.Contains(searchTerm) ||
                        u.Email.Contains(searchTerm));
                }

                var users = await usersQuery.OrderByDescending(u => u.CreatedAt).ToListAsync();

                var viewModel = users.Select(u => new UserManagementViewModel
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Email = u.Email,
                    Role = u.Role,
                    Status = u.Status,
                    CreatedAt = u.CreatedAt
                }).ToList();

                return View(viewModel);
            }
            catch (Exception ex)
            {
                // This error typically means the database schema is missing columns
                TempData["ErrorMessage"] = "Database Schema Error: " + ex.Message + 
                    "\n\nPlease run: Update-Database in Package Manager Console";
                return RedirectToAction("Dashboard");
            }
        }

        // GET: Admin/CreateUser
        public IActionResult CreateUser()
        {
            return View();
        }

        // POST: Admin/CreateUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(CreateUserViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            if (await _context.Users.AnyAsync(u => u.Email == model.Email))
            {
                ModelState.AddModelError("Email", "This email is already registered.");
                return View(model);
            }

            var user = new User
            {
                FullName = model.FullName,
                Email = model.Email,
                Password = HashPassword(model.Password), // Encrypting for security
                Role = model.Role,
                RoleId = model.RoleId ?? 0,
                Status = "Active",
                CreatedAt = DateTime.Now
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            await LogSystemActivity(0, "Create User", "UserTable", $"Created user: {model.FullName}");

            return RedirectToAction(nameof(ManageUsers));
        }

        // GET: Admin/EditUser/5
        public async Task<IActionResult> EditUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            var model = new EditUserViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                RoleId = user.RoleId,
                Status = user.Status
            };

            return View(model);
        }

        // POST: Admin/EditUser/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(int id, EditUserViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null) return NotFound();

                user.FullName = model.FullName;
                user.Email = model.Email;
                user.Role = model.Role;
                user.RoleId = model.RoleId ?? 0;
                user.Status = model.Status;

                _context.Update(user);
                await _context.SaveChangesAsync();

                await LogSystemActivity(0, "Edit User", "UserTable", $"Updated user ID: {id}");
                return RedirectToAction(nameof(ManageUsers));
            }
            return View(model);
        }

        // Deactivate User
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeactivateUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            user.Status = "Inactive";
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            await LogSystemActivity(0, "Deactivate User", "UserTable", $"Deactivated user: {user.FullName}");

            return RedirectToAction(nameof(ManageUsers));
        }

        // GET: Admin/ManageRoles
        public async Task<IActionResult> ManageRoles()
        {
            var roles = await _context.Roles.ToListAsync();
            var viewModel = new List<RoleManagementViewModel>();

            foreach (var role in roles)
            {
                viewModel.Add(new RoleManagementViewModel
                {
                    RoleId = role.RoleId,
                    RoleName = role.RoleName,
                    Description = role.Description,
                    UserCount = await _context.Users.CountAsync(u => u.RoleId == role.RoleId)
                });
            }

            return View(viewModel);
        }

        // GET: Admin/CreateRole
        public IActionResult CreateRole()
        {
            return View();
        }

        // POST: Admin/CreateRole
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(Role model)
        {
            if (!ModelState.IsValid) return View(model);

            if (await _context.Roles.AnyAsync(r => r.RoleName == model.RoleName))
            {
                ModelState.AddModelError("RoleName", "Role name already exists");
                return View(model);
            }

            _context.Roles.Add(model);
            await _context.SaveChangesAsync();

            await LogSystemActivity(0, "Create Role", "RoleTable", $"Created role: {model.RoleName}");

            return RedirectToAction(nameof(ManageRoles));
        }

        // GET: Admin/EditRole/5
        public async Task<IActionResult> EditRole(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null) return NotFound();

            return View(role);
        }

        // POST: Admin/EditRole/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRole(int id, Role model)
        {
            if (id != model.RoleId) return NotFound();

            if (!ModelState.IsValid) return View(model);

            var role = await _context.Roles.FindAsync(id);
            if (role == null) return NotFound();

            role.RoleName = model.RoleName;
            role.Description = model.Description;

            _context.Roles.Update(role);
            await _context.SaveChangesAsync();

            await LogSystemActivity(0, "Edit Role", "RoleTable", $"Updated role: {model.RoleName}");

            return RedirectToAction(nameof(ManageRoles));
        }

        // DELETE: Admin/DeleteRole/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null) return NotFound();

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();

            await LogSystemActivity(0, "Delete Role", "RoleTable", $"Deleted role: {role.RoleName}");

            return RedirectToAction(nameof(ManageRoles));
        }

        // GET: Admin/SystemMonitoring
        public async Task<IActionResult> SystemMonitoring(DateTime? fromDate, DateTime? toDate, string filterAction = "")
        {
            var logsQuery = _context.SystemLogs.AsQueryable();

            if (fromDate.HasValue)
            {
                logsQuery = logsQuery.Where(l => l.Timestamp >= fromDate);
            }

            if (toDate.HasValue)
            {
                toDate = toDate.Value.AddDays(1);
                logsQuery = logsQuery.Where(l => l.Timestamp <= toDate);
            }

            if (!string.IsNullOrEmpty(filterAction))
            {
                logsQuery = logsQuery.Where(l => l.Action.Contains(filterAction));
            }

            var logs = await logsQuery
                .OrderByDescending(l => l.Timestamp)
                .Take(100)
                .ToListAsync();

            var activityList = logs.Select(l => new SystemActivityViewModel
            {
                LogId = l.LogId,
                UserId = l.UserId,
                Action = l.Action,
                Resource = l.Resource,
                Timestamp = l.Timestamp,
                IpAddress = l.IpAddress
            }).ToList();

            var model = new SystemMonitoringViewModel
            {
                TotalActivities = await _context.SystemLogs.CountAsync(),
                TodayActivities = await _context.SystemLogs
                    .Where(l => l.Timestamp.Date == DateTime.Now.Date)
                    .CountAsync(),
                RecentActivities = activityList,
                FilterFromDate = fromDate,
                FilterToDate = toDate,
                FilterAction = filterAction
            };

            return View(model);
        }

        // GET: Admin/Reports
        public async Task<IActionResult> Reports(string reportType = "")
        {
            var reportsQuery = _context.Reports.AsQueryable();

            if (!string.IsNullOrEmpty(reportType) && reportType != "All")
            {
                reportsQuery = reportsQuery.Where(r => r.ReportType == reportType);
            }

            var reports = await reportsQuery
                .OrderByDescending(r => r.GeneratedDate)
                .ToListAsync();

            var viewModel = reports.Select(r => new ReportListViewModel
            {
                ReportId = r.ReportId,
                ReportName = r.ReportName,
                ReportType = r.ReportType,
                GeneratedDate = r.GeneratedDate,
                GeneratedByName = "Admin",
                StartDate = r.StartDate,
                EndDate = r.EndDate
            }).ToList();

            return View(viewModel);
        }

        // GET: Admin/GenerateReport
        public IActionResult GenerateReport()
        {
            return View();
        }

        // POST: Admin/GenerateReport
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GenerateReport(ReportGenerationViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            string reportContent = GenerateReportContent(model.ReportType, model.StartDate, model.EndDate);

            var report = new Report
            {
                ReportName = model.ReportName,
                ReportType = model.ReportType,
                GeneratedBy = 0,
                GeneratedDate = DateTime.Now,
                ReportContent = reportContent,
                StartDate = model.StartDate,
                EndDate = model.EndDate
            };

            _context.Reports.Add(report);
            await _context.SaveChangesAsync();

            await LogSystemActivity(0, "Generate Report", "ReportTable", $"Generated report: {model.ReportName}");

            return RedirectToAction(nameof(Reports));
        }

        // GET: Admin/ViewReport/5
        public async Task<IActionResult> ViewReport(int id)
        {
            var report = await _context.Reports.FindAsync(id);
            if (report == null) return NotFound();

            return View(report);
        }

        // --- HELPER METHODS ---

        private async Task LogSystemActivity(int userId, string action, string resource, string description)
        {
            var log = new SystemLog
            {
                UserId = userId,
                Action = action,
                Resource = resource,
                Timestamp = DateTime.Now,
                IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1"
            };

            _context.SystemLogs.Add(log);
            await _context.SaveChangesAsync();
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        private string GenerateReportContent(string reportType, DateTime? startDate, DateTime? endDate)
        {
            return reportType switch
            {
                "Employment" => $"Employment Report - Period: {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}",
                "Compliance" => $"Compliance Report - Period: {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}",
                "Participation" => $"Participation Report - Period: {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}",
                _ => "System Report"
            };
        }
    }
}
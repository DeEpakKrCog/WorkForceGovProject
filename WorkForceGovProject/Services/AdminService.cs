using WorkForceGovProject.Interfaces;
using WorkForceGovProject.Models;
using WorkForceGovProject.Models.ViewModels;

namespace WorkForceGovProject.Services
{
    /// <summary>
    /// Admin Service - High-level admin operations using AdminRepository
    /// Provides business logic and orchestration for admin module
    /// </summary>
    public interface IAdminService
    {
        // Dashboard
        Task<AdminDashboardViewModel> GetDashboardDataAsync();

        // User Management
        Task<IEnumerable<UserManagementViewModel>> GetUsersAsync(string status = "", string searchTerm = "");
        Task<bool> CreateUserAsync(CreateUserViewModel model);
        Task<bool> UpdateUserAsync(int userId, EditUserViewModel model);
        Task<bool> DeactivateUserAsync(int userId);
        Task<bool> ActivateUserAsync(int userId);

        // Role Management
        Task<IEnumerable<RoleManagementViewModel>> GetRolesAsync();
        Task<bool> CreateRoleAsync(Role role);
        Task<bool> UpdateRoleAsync(Role role);
        Task<bool> DeleteRoleAsync(int roleId);

        // Report Management
        Task<IEnumerable<ReportListViewModel>> GetReportsAsync(string reportType = "");
        Task<bool> GenerateReportAsync(ReportGenerationViewModel model, int generatedBy);
        Task<bool> DeleteReportAsync(int reportId);

        // System Monitoring
        Task<SystemMonitoringViewModel> GetMonitoringDataAsync(DateTime? fromDate, DateTime? toDate, string filterAction = "");
    }

    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;

        public AdminService(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        // ===== DASHBOARD =====

        public async Task<AdminDashboardViewModel> GetDashboardDataAsync()
        {
            var stats = await _adminRepository.GetAdminStatisticsAsync();

            return new AdminDashboardViewModel
            {
                TotalUsers = stats.TotalUsers,
                ActiveUsers = stats.ActiveUsers,
                InactiveUsers = stats.InactiveUsers,
                TotalRoles = stats.TotalRoles,
                RecentUsers = stats.RecentUsers,
                RecentLogs = stats.RecentLogs
            };
        }

        // ===== USER MANAGEMENT =====

        public async Task<IEnumerable<UserManagementViewModel>> GetUsersAsync(string status = "", string searchTerm = "")
        {
            IEnumerable<User> users;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                users = await _adminRepository.SearchUsersAsync(searchTerm);
            }
            else if (!string.IsNullOrEmpty(status) && status != "All")
            {
                users = await _adminRepository.GetUsersByStatusAsync(status);
            }
            else
            {
                users = await _adminRepository.GetAllUsersAsync();
            }

            return users.Select(MapUserToViewModel);
        }

        public async Task<bool> CreateUserAsync(CreateUserViewModel model)
        {
            if (!await _adminRepository.IsEmailUniqueAsync(model.Email))
                return false;

            var user = new User
            {
                FullName = model.FullName,
                Email = model.Email,
                Password = HashPassword(model.Password),
                Role = model.Role,
                RoleId = model.RoleId ?? 0,
                Status = "Active"
            };

            return await _adminRepository.CreateUserAsync(user);
        }

        public async Task<bool> UpdateUserAsync(int userId, EditUserViewModel model)
        {
            var user = await _adminRepository.GetUserByIdAsync(userId);
            if (user == null)
                return false;

            if (user.Email != model.Email && !await _adminRepository.IsEmailUniqueAsync(model.Email, userId))
                return false;

            user.FullName = model.FullName;
            user.Email = model.Email;
            user.Role = model.Role;
            user.RoleId = model.RoleId ?? 0;
            user.Status = model.Status;

            return await _adminRepository.UpdateUserAsync(user);
        }

        public async Task<bool> DeactivateUserAsync(int userId)
        {
            var user = await _adminRepository.GetUserByIdAsync(userId);
            if (user == null)
                return false;

            user.Status = "Inactive";
            return await _adminRepository.UpdateUserAsync(user);
        }

        public async Task<bool> ActivateUserAsync(int userId)
        {
            var user = await _adminRepository.GetUserByIdAsync(userId);
            if (user == null)
                return false;

            user.Status = "Active";
            return await _adminRepository.UpdateUserAsync(user);
        }

        // ===== ROLE MANAGEMENT =====

        public async Task<IEnumerable<RoleManagementViewModel>> GetRolesAsync()
        {
            var roles = await _adminRepository.GetAllRolesAsync();

            var result = new List<RoleManagementViewModel>();
            foreach (var role in roles)
            {
                var userCount = await _adminRepository.GetUsersCountByRoleAsync(role.RoleId);
                result.Add(new RoleManagementViewModel
                {
                    RoleId = role.RoleId,
                    RoleName = role.RoleName,
                    Description = role.Description,
                    UserCount = userCount
                });
            }

            return result;
        }

        public async Task<bool> CreateRoleAsync(Role role)
        {
            if (!await _adminRepository.IsRoleNameUniqueAsync(role.RoleName))
                return false;

            return await _adminRepository.CreateRoleAsync(role);
        }

        public async Task<bool> UpdateRoleAsync(Role role)
        {
            if (!await _adminRepository.IsRoleNameUniqueAsync(role.RoleName, role.RoleId))
                return false;

            return await _adminRepository.UpdateRoleAsync(role);
        }

        public async Task<bool> DeleteRoleAsync(int roleId)
        {
            return await _adminRepository.DeleteRoleAsync(roleId);
        }

        // ===== REPORT MANAGEMENT =====

        public async Task<IEnumerable<ReportListViewModel>> GetReportsAsync(string reportType = "")
        {
            IEnumerable<Report> reports;

            if (!string.IsNullOrEmpty(reportType) && reportType != "All")
            {
                reports = await _adminRepository.GetReportsByTypeAsync(reportType);
            }
            else
            {
                reports = await _adminRepository.GetAllReportsAsync();
            }

            return reports.Select(r => new ReportListViewModel
            {
                ReportId = r.ReportId,
                ReportName = r.ReportName,
                ReportType = r.ReportType,
                GeneratedDate = r.GeneratedDate,
                GeneratedByName = "Admin",
                StartDate = r.StartDate,
                EndDate = r.EndDate
            });
        }

        public async Task<bool> GenerateReportAsync(ReportGenerationViewModel model, int generatedBy)
        {
            var report = new Report
            {
                ReportName = model.ReportName,
                ReportType = model.ReportType,
                GeneratedBy = generatedBy,
                ReportContent = GenerateReportContent(model.ReportType),
                StartDate = model.StartDate,
                EndDate = model.EndDate
            };

            return await _adminRepository.CreateReportAsync(report);
        }

        public async Task<bool> DeleteReportAsync(int reportId)
        {
            return await _adminRepository.DeleteReportAsync(reportId);
        }

        // ===== SYSTEM MONITORING =====

        public async Task<SystemMonitoringViewModel> GetMonitoringDataAsync(DateTime? fromDate, DateTime? toDate, string filterAction = "")
        {
            IEnumerable<SystemLog> activities;

            if (!string.IsNullOrEmpty(filterAction) && fromDate.HasValue && toDate.HasValue)
            {
                activities = await _adminRepository.GetLogsByActionAndDateAsync(filterAction, fromDate.Value, toDate.Value.AddDays(1));
            }
            else if (fromDate.HasValue && toDate.HasValue)
            {
                activities = await _adminRepository.GetLogsByDateRangeAsync(fromDate.Value, toDate.Value.AddDays(1));
            }
            else if (!string.IsNullOrEmpty(filterAction))
            {
                activities = await _adminRepository.GetLogsByActionAsync(filterAction);
            }
            else
            {
                activities = await _adminRepository.GetRecentLogsAsync();
            }

            return new SystemMonitoringViewModel
            {
                TotalActivities = await _adminRepository.GetTotalLogsCountAsync(),
                TodayActivities = await _adminRepository.GetTodayLogsCountAsync(),
                RecentActivities = activities.OrderByDescending(a => a.Timestamp).Take(100)
                    .Select(l => new SystemActivityViewModel
                    {
                        LogId = l.LogId,
                        UserId = l.UserId,
                        Action = l.Action,
                        Resource = l.Resource,
                        Timestamp = l.Timestamp,
                        IpAddress = l.IpAddress
                    }).ToList(),
                FilterFromDate = fromDate,
                FilterToDate = toDate,
                FilterAction = filterAction
            };
        }

        // ===== HELPER METHODS =====

        private UserManagementViewModel MapUserToViewModel(User user)
        {
            return new UserManagementViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                Status = user.Status,
                CreatedAt = user.CreatedAt
            };
        }

        private string HashPassword(string password)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        private string GenerateReportContent(string reportType)
        {
            return reportType switch
            {
                "Employment" => $"Employment Report - Generated: {DateTime.Now:yyyy-MM-dd}",
                "Compliance" => $"Compliance Report - Generated: {DateTime.Now:yyyy-MM-dd}",
                "Participation" => $"Participation Report - Generated: {DateTime.Now:yyyy-MM-dd}",
                _ => "System Report"
            };
        }
    }
}

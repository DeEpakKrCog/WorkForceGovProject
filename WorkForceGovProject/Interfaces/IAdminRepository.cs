using WorkForceGovProject.Models;

namespace WorkForceGovProject.Interfaces
{
    /// <summary>
    /// Admin Repository Interface - Coordinates all admin-related data operations
    /// Provides unified access to User, Role, Report, and SystemLog repositories
    /// </summary>
    public interface IAdminRepository
    {
        // ===== USER OPERATIONS =====
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int userId);
        Task<User> GetUserByEmailAsync(string email);
        Task<IEnumerable<User>> GetUsersByStatusAsync(string status);
        Task<IEnumerable<User>> SearchUsersAsync(string searchTerm);
        Task<int> GetTotalUsersCountAsync();
        Task<int> GetActiveUsersCountAsync();
        Task<int> GetInactiveUsersCountAsync();
        Task<bool> CreateUserAsync(User user);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(int userId);
        Task<bool> IsEmailUniqueAsync(string email, int? excludeUserId = null);

        // ===== ROLE OPERATIONS =====
        Task<IEnumerable<Role>> GetAllRolesAsync();
        Task<Role> GetRoleByIdAsync(int roleId);
        Task<Role> GetRoleByNameAsync(string roleName);
        Task<int> GetTotalRolesCountAsync();
        Task<int> GetUsersCountByRoleAsync(int roleId);
        Task<bool> CreateRoleAsync(Role role);
        Task<bool> UpdateRoleAsync(Role role);
        Task<bool> DeleteRoleAsync(int roleId);
        Task<bool> IsRoleNameUniqueAsync(string roleName, int? excludeRoleId = null);

        // ===== REPORT OPERATIONS =====
        Task<IEnumerable<Report>> GetAllReportsAsync();
        Task<Report> GetReportByIdAsync(int reportId);
        Task<IEnumerable<Report>> GetReportsByTypeAsync(string reportType);
        Task<IEnumerable<Report>> GetReportsByDateRangeAsync(DateTime fromDate, DateTime toDate);
        Task<IEnumerable<Report>> GetRecentReportsAsync(int count = 50);
        Task<int> GetTotalReportsCountAsync();
        Task<bool> CreateReportAsync(Report report);
        Task<bool> DeleteReportAsync(int reportId);

        // ===== SYSTEM LOG OPERATIONS =====
        Task<IEnumerable<SystemLog>> GetAllLogsAsync();
        Task<IEnumerable<SystemLog>> GetLogsByUserAsync(int userId);
        Task<IEnumerable<SystemLog>> GetLogsByActionAsync(string action);
        Task<IEnumerable<SystemLog>> GetLogsByDateRangeAsync(DateTime fromDate, DateTime toDate);
        Task<IEnumerable<SystemLog>> GetLogsByActionAndDateAsync(string action, DateTime fromDate, DateTime toDate);
        Task<IEnumerable<SystemLog>> GetRecentLogsAsync(int count = 100);
        Task<int> GetTotalLogsCountAsync();
        Task<int> GetTodayLogsCountAsync();
        Task<bool> LogActivityAsync(SystemLog log);

        // ===== ADMIN DASHBOARD STATISTICS =====
        Task<AdminStatistics> GetAdminStatisticsAsync();
    }

    /// <summary>
    /// Admin Statistics DTO - Contains dashboard statistics
    /// </summary>
    public class AdminStatistics
    {
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int InactiveUsers { get; set; }
        public int TotalRoles { get; set; }
        public int TotalReports { get; set; }
        public int TodayLogs { get; set; }
        public int TotalLogs { get; set; }
        public List<User> RecentUsers { get; set; } = new();
        public List<SystemLog> RecentLogs { get; set; } = new();
    }
}

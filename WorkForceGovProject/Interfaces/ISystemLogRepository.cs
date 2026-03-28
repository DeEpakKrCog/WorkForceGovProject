using WorkForceGovProject.Models;

namespace WorkForceGovProject.Interfaces
{
    public interface ISystemLogRepository : IRepository<SystemLog>
    {
        Task<IEnumerable<SystemLog>> GetLogsByUserAsync(int userId);
        Task<IEnumerable<SystemLog>> GetLogsByActionAsync(string action);
        Task<IEnumerable<SystemLog>> GetLogsByDateRangeAsync(DateTime fromDate, DateTime toDate);
        Task<IEnumerable<SystemLog>> GetLogsByActionAndDateAsync(string action, DateTime fromDate, DateTime toDate);
        Task<IEnumerable<SystemLog>> GetRecentLogsAsync(int count = 100);
        Task<int> GetTodayLogsCountAsync();
        Task<IEnumerable<SystemLog>> SearchLogsAsync(string searchTerm);
    }
}

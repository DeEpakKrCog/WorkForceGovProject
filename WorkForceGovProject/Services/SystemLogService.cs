using WorkForceGovProject.Interfaces;
using WorkForceGovProject.Models;
using WorkForceGovProject.Models.ViewModels;

namespace WorkForceGovProject.Services
{
    public class SystemLogService : ISystemLogService
    {
        private readonly ISystemLogRepository _logRepository;

        public SystemLogService(ISystemLogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        // Log Queries
        public async Task<IEnumerable<SystemActivityViewModel>> GetAllLogsAsync()
        {
            var logs = await _logRepository.GetAllAsync();
            return logs.Select(MapToViewModel);
        }

        public async Task<IEnumerable<SystemActivityViewModel>> GetLogsByUserAsync(int userId)
        {
            var logs = await _logRepository.GetLogsByUserAsync(userId);
            return logs.Select(MapToViewModel);
        }

        public async Task<IEnumerable<SystemActivityViewModel>> GetLogsByActionAsync(string action)
        {
            var logs = await _logRepository.GetLogsByActionAsync(action);
            return logs.Select(MapToViewModel);
        }

        public async Task<IEnumerable<SystemActivityViewModel>> GetLogsByDateRangeAsync(DateTime fromDate, DateTime toDate)
        {
            var logs = await _logRepository.GetLogsByDateRangeAsync(fromDate, toDate);
            return logs.Select(MapToViewModel);
        }

        public async Task<IEnumerable<SystemActivityViewModel>> GetLogsByActionAndDateAsync(string action, DateTime fromDate, DateTime toDate)
        {
            var logs = await _logRepository.GetLogsByActionAndDateAsync(action, fromDate, toDate);
            return logs.Select(MapToViewModel);
        }

        public async Task<IEnumerable<SystemActivityViewModel>> GetRecentLogsAsync(int count = 100)
        {
            var logs = await _logRepository.GetRecentLogsAsync(count);
            return logs.Select(MapToViewModel);
        }

        public async Task<int> GetTotalLogsCountAsync()
        {
            return await _logRepository.CountAsync();
        }

        public async Task<int> GetTodayLogsCountAsync()
        {
            return await _logRepository.GetTodayLogsCountAsync();
        }

        // Log Commands
        public async Task<bool> LogActivityAsync(int userId, string action, string resource, string ipAddress = "127.0.0.1")
        {
            try
            {
                var log = new SystemLog
                {
                    UserId = userId,
                    Action = action,
                    Resource = resource,
                    Timestamp = DateTime.Now,
                    IpAddress = ipAddress
                };

                await _logRepository.AddAsync(log);
                await _logRepository.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        // Helper Methods
        private SystemActivityViewModel MapToViewModel(SystemLog log)
        {
            return new SystemActivityViewModel
            {
                LogId = log.LogId,
                UserId = log.UserId,
                Action = log.Action,
                Resource = log.Resource,
                Timestamp = log.Timestamp,
                IpAddress = log.IpAddress
            };
        }
    }
}

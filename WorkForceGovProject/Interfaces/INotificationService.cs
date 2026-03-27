using WorkForceGovProject.Models;

namespace WorkForceGovProject.Interfaces
{
    /// <summary>
    /// Notification Service Interface
    /// Defines notification operations
    /// </summary>
    public interface INotificationService
    {
        Task<IEnumerable<Notification>> GetUserNotificationsAsync(int userId);
        Task<IEnumerable<Notification>> GetUnreadNotificationsAsync(int userId);
        Task<int> GetUnreadCountAsync(int userId);
        Task<(bool Success, Notification Notification)> CreateNotificationAsync(int userId, string message, string category, int? entityId = null, string entityType = null);
        Task<(bool Success, IEnumerable<Notification>)> CreateNotificationsAsync(List<(int userId, string message, string category, int? entityId, string entityType)> notifications);
        Task<(bool Success, string Message)> MarkAsReadAsync(int notificationId);
        Task<(bool Success, string Message)> MarkAllAsReadAsync(int userId);
        Task<(bool Success, string Message)> DeleteNotificationAsync(int notificationId);
    }
}

using WorkForceGovProject.Models;

namespace WorkForceGovProject.Interfaces
{
    public interface INotificationRepository
    {
        // Create
        Task<Notification> CreateNotificationAsync(Notification notification);
        Task<IEnumerable<Notification>> CreateBulkNotificationsAsync(IEnumerable<Notification> notifications);

        // Read
        Task<Notification> GetNotificationByIdAsync(int id);
        Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(int userId);
        Task<IEnumerable<Notification>> GetNotificationsByEmployerIdAsync(int employerId);
        Task<IEnumerable<Notification>> GetUnreadNotificationsByUserIdAsync(int userId);
        Task<IEnumerable<Notification>> GetNotificationsByCategoryAsync(string category);
        Task<IEnumerable<Notification>> GetRecentNotificationsAsync(int userId, int count);

        // Update
        Task<Notification> UpdateNotificationAsync(Notification notification);
        Task<bool> MarkAsReadAsync(int id);
        Task<bool> MarkAllAsReadAsync(int userId);

        // Delete
        Task<bool> DeleteNotificationAsync(int id);
        Task<bool> DeleteAllNotificationsByUserIdAsync(int userId);

        // Additional methods
        Task<bool> NotificationExistsAsync(int id);
        Task<int> GetUnreadCountAsync(int userId);
        Task<IEnumerable<Notification>> GetNotificationsByDateRangeAsync(int userId, DateTime startDate, DateTime endDate);
    }
}

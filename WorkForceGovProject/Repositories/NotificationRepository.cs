using Microsoft.EntityFrameworkCore;
using WorkForceGovProject.Data;
using WorkForceGovProject.Interfaces;
using WorkForceGovProject.Models;

namespace WorkForceGovProject.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ApplicationDbContext _context;

        public NotificationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Notification> CreateNotificationAsync(Notification notification)
        {
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
            return notification;
        }

        public async Task<IEnumerable<Notification>> CreateBulkNotificationsAsync(IEnumerable<Notification> notifications)
        {
            _context.Notifications.AddRange(notifications);
            await _context.SaveChangesAsync();
            return notifications;
        }

        public async Task<Notification> GetNotificationByIdAsync(int id)
        {
            return await _context.Notifications.FindAsync(id);
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(int userId)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByEmployerIdAsync(int employerId)
        {
            return await _context.Notifications
                .Where(n => n.EmployerId == employerId)
                .OrderByDescending(n => n.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Notification>> GetUnreadNotificationsByUserIdAsync(int userId)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId && n.Status == "Unread")
                .OrderByDescending(n => n.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByCategoryAsync(string category)
        {
            return await _context.Notifications
                .Where(n => n.Category == category)
                .OrderByDescending(n => n.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Notification>> GetRecentNotificationsAsync(int userId, int count)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedDate)
                .Take(count)
                .ToListAsync();
        }

        public async Task<Notification> UpdateNotificationAsync(Notification notification)
        {
            _context.Notifications.Update(notification);
            await _context.SaveChangesAsync();
            return notification;
        }

        public async Task<bool> MarkAsReadAsync(int id)
        {
            var notification = await GetNotificationByIdAsync(id);
            if (notification == null) return false;

            notification.Status = "Read";
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> MarkAllAsReadAsync(int userId)
        {
            var notifications = await _context.Notifications
                .Where(n => n.UserId == userId && n.Status == "Unread")
                .ToListAsync();

            foreach (var notification in notifications)
            {
                notification.Status = "Read";
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteNotificationAsync(int id)
        {
            var notification = await GetNotificationByIdAsync(id);
            if (notification == null) return false;

            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAllNotificationsByUserIdAsync(int userId)
        {
            var notifications = await _context.Notifications
                .Where(n => n.UserId == userId)
                .ToListAsync();

            _context.Notifications.RemoveRange(notifications);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> NotificationExistsAsync(int id)
        {
            return await _context.Notifications.AnyAsync(n => n.Id == id);
        }

        public async Task<int> GetUnreadCountAsync(int userId)
        {
            return await _context.Notifications
                .CountAsync(n => n.UserId == userId && n.Status == "Unread");
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByDateRangeAsync(int userId, DateTime startDate, DateTime endDate)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId && 
                           n.CreatedDate >= startDate && 
                           n.CreatedDate <= endDate)
                .OrderByDescending(n => n.CreatedDate)
                .ToListAsync();
        }
    }
}

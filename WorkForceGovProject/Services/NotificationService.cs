using WorkForceGovProject.Interfaces;
using WorkForceGovProject.Models;

namespace WorkForceGovProject.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IApplicationRepository _applicationRepository;
        private readonly ICitizenRepository _citizenRepository;
        private readonly IJobOpeningRepository _jobOpeningRepository;
        private readonly IEmployerRepository _employerRepository;

        public NotificationService(
            INotificationRepository notificationRepository,
            IApplicationRepository applicationRepository,
            ICitizenRepository citizenRepository,
            IJobOpeningRepository jobOpeningRepository,
            IEmployerRepository employerRepository)
        {
            _notificationRepository = notificationRepository;
            _applicationRepository = applicationRepository;
            _citizenRepository = citizenRepository;
            _jobOpeningRepository = jobOpeningRepository;
            _employerRepository = employerRepository;
        }

        public async Task<Notification> CreateNotificationAsync(int userId, string message, string category, int? entityId = null, int? employerId = null)
        {
            var notification = new Notification
            {
                UserId = userId,
                EntityId = entityId,
                Message = message,
                Category = category,
                Status = "Unread",
                CreatedDate = DateTime.Now,
                EmployerId = employerId
            };

            return await _notificationRepository.CreateNotificationAsync(notification);
        }

        public async Task CreateApplicationStatusNotificationAsync(int applicationId, string status)
        {
            var application = await _applicationRepository.GetApplicationByIdAsync(applicationId);
            if (application == null) return;

            var citizen = await _citizenRepository.GetCitizenByIdAsync(application.CitizenId);
            if (citizen == null) return;

            var job = await _jobOpeningRepository.GetJobOpeningByIdAsync(application.JobId);
            if (job == null) return;

            string message = status switch
            {
                "Shortlisted" => $"Your application for '{job.Title}' has been shortlisted!",
                "Approved" => $"Congratulations! You have been hired for '{job.Title}'!",
                "Rejected" => $"Your application for '{job.Title}' has been reviewed.",
                _ => $"Your application status for '{job.Title}' has been updated to {status}."
            };

            string category = status == "Approved" ? "Hiring" : "Application Update";

            await CreateNotificationAsync(citizen.UserId, message, category, applicationId);
        }

        public async Task CreateJobApplicationNotificationAsync(int jobId, int employerId)
        {
            var job = await _jobOpeningRepository.GetJobOpeningByIdAsync(jobId);
            if (job == null) return;

            var employer = await _employerRepository.GetEmployerByIdAsync(employerId);
            if (employer == null) return;

            var applicationsCount = await _applicationRepository.GetApplicationsCountByJobIdAsync(jobId);

            string message = $"New application received for '{job.Title}'. Total applications: {applicationsCount}";

            await CreateNotificationAsync(employer.UserId, message, "Job Application", jobId, employerId);
        }

        public async Task CreateDocumentVerificationNotificationAsync(int employerId, string status)
        {
            var employer = await _employerRepository.GetEmployerByIdAsync(employerId);
            if (employer == null) return;

            string message = status == "Verified"
                ? "Your document has been verified successfully!"
                : "Your document verification requires attention.";

            await CreateNotificationAsync(employer.UserId, message, "Document Verification", null, employerId);
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(int userId)
        {
            return await _notificationRepository.GetNotificationsByUserIdAsync(userId);
        }

        public async Task<IEnumerable<Notification>> GetUnreadNotificationsByUserIdAsync(int userId)
        {
            return await _notificationRepository.GetUnreadNotificationsByUserIdAsync(userId);
        }

        public async Task<IEnumerable<Notification>> GetRecentNotificationsAsync(int userId, int count)
        {
            return await _notificationRepository.GetRecentNotificationsAsync(userId, count);
        }

        public async Task<bool> MarkAsReadAsync(int notificationId)
        {
            return await _notificationRepository.MarkAsReadAsync(notificationId);
        }

        public async Task<bool> MarkAllAsReadAsync(int userId)
        {
            return await _notificationRepository.MarkAllAsReadAsync(userId);
        }

        public async Task<bool> DeleteNotificationAsync(int notificationId)
        {
            return await _notificationRepository.DeleteNotificationAsync(notificationId);
        }

        public async Task<int> GetUnreadCountAsync(int userId)
        {
            return await _notificationRepository.GetUnreadCountAsync(userId);
        }
    }
}

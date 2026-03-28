using WorkForceGovProject.Interfaces;
using WorkForceGovProject.Models;

namespace WorkForceGovProject.Services
{
    public class EmployerService : IEmployerService
    {
        private readonly IEmployerRepository _employerRepository;
        private readonly IUserRepository _userRepository;
        private readonly IJobOpeningRepository _jobOpeningRepository;
        private readonly IApplicationRepository _applicationRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IEmployerDocumentRepository _employerDocumentRepository;

        public EmployerService(
            IEmployerRepository employerRepository,
            IUserRepository userRepository,
            IJobOpeningRepository jobOpeningRepository,
            IApplicationRepository applicationRepository,
            INotificationRepository notificationRepository,
            IEmployerDocumentRepository employerDocumentRepository)
        {
            _employerRepository = employerRepository;
            _userRepository = userRepository;
            _jobOpeningRepository = jobOpeningRepository;
            _applicationRepository = applicationRepository;
            _notificationRepository = notificationRepository;
            _employerDocumentRepository = employerDocumentRepository;
        }

        public async Task<(bool Success, string Message, Employer Employer)> RegisterEmployerAsync(Employer employer, string email, string password)
        {
            // Validate email
            if (await _userRepository.UserExistsAsync(email))
            {
                return (false, "This email is already registered.", null);
            }

            // Validate employer data
            if (string.IsNullOrWhiteSpace(employer.CompanyName))
            {
                return (false, "Company name is required.", null);
            }

            if (string.IsNullOrWhiteSpace(employer.Industry))
            {
                return (false, "Industry is required.", null);
            }

            // Create user account
            var user = new User
            {
                FullName = employer.CompanyName,
                Email = email,
                Password = password, // Note: In production, hash the password
                Role = "Employer"
            };

            var createdUser = await _userRepository.CreateUserAsync(user);

            // Create employer profile
            employer.UserId = createdUser.Id;
            employer.RegistrationDate = DateTime.Now;
            employer.Status = "Pending";

            var createdEmployer = await _employerRepository.CreateEmployerAsync(employer);

            return (true, "Registration successful! Your account is pending verification.", createdEmployer);
        }

        public async Task<Employer> GetEmployerByIdAsync(int id)
        {
            return await _employerRepository.GetEmployerByIdAsync(id);
        }

        public async Task<Employer> GetEmployerByUserIdAsync(int userId)
        {
            return await _employerRepository.GetEmployerByUserIdAsync(userId);
        }

        public async Task<(bool Success, string Message)> UpdateEmployerProfileAsync(Employer employer)
        {
            var existingEmployer = await _employerRepository.GetEmployerByIdAsync(employer.Id);
            if (existingEmployer == null)
            {
                return (false, "Employer not found.");
            }

            // Update fields
            existingEmployer.CompanyName = employer.CompanyName;
            existingEmployer.Industry = employer.Industry;
            existingEmployer.Address = employer.Address;
            existingEmployer.ContactInfo = employer.ContactInfo;

            await _employerRepository.UpdateEmployerAsync(existingEmployer);

            return (true, "Profile updated successfully!");
        }

        public async Task<bool> IsEmployerVerifiedAsync(int employerId)
        {
            var employer = await _employerRepository.GetEmployerByIdAsync(employerId);
            return employer?.Status == "Active" || employer?.Status == "Verified";
        }

        public async Task<EmployerDashboardData> GetDashboardDataAsync(int employerId)
        {
            var employer = await _employerRepository.GetEmployerByIdAsync(employerId);
            if (employer == null)
            {
                return null;
            }

            var jobs = await _jobOpeningRepository.GetJobOpeningsByEmployerIdAsync(employerId);
            var activeJobsCount = await _jobOpeningRepository.GetActiveJobsCountByEmployerAsync(employerId);
            var applications = await _applicationRepository.GetApplicationsByEmployerIdAsync(employerId);
            var pendingAppsCount = await _applicationRepository.GetPendingApplicationsCountAsync(employerId);
            var unreadNotifs = await _notificationRepository.GetUnreadCountAsync(employer.UserId);
            var pendingDocs = await _employerDocumentRepository.GetPendingDocumentsCountAsync();

            return new EmployerDashboardData
            {
                TotalJobPostings = jobs.Count(),
                ActiveJobPostings = activeJobsCount,
                TotalApplicants = applications.Count(),
                PendingApplications = pendingAppsCount,
                ShortlistedCandidates = applications.Count(a => a.Status == "Shortlisted"),
                HiredCandidates = applications.Count(a => a.Status == "Approved"),
                UnreadNotifications = unreadNotifs,
                PendingDocuments = pendingDocs
            };
        }

        public async Task<IEnumerable<Employer>> GetEmployersByIndustryAsync(string industry)
        {
            return await _employerRepository.GetEmployersByIndustryAsync(industry);
        }

        public async Task<IEnumerable<Employer>> GetEmployersByStatusAsync(string status)
        {
            return await _employerRepository.GetEmployersByStatusAsync(status);
        }
    }
}

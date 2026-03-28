using WorkForceGovProject.Models;

namespace WorkForceGovProject.Services
{
    public interface IEmployerService
    {
        // Registration & Profile
        Task<(bool Success, string Message, Employer Employer)> RegisterEmployerAsync(Employer employer, string email, string password);
        Task<Employer> GetEmployerByIdAsync(int id);
        Task<Employer> GetEmployerByUserIdAsync(int userId);
        Task<(bool Success, string Message)> UpdateEmployerProfileAsync(Employer employer);
        Task<bool> IsEmployerVerifiedAsync(int employerId);

        // Dashboard Statistics
        Task<EmployerDashboardData> GetDashboardDataAsync(int employerId);

        // Search & Filter
        Task<IEnumerable<Employer>> GetEmployersByIndustryAsync(string industry);
        Task<IEnumerable<Employer>> GetEmployersByStatusAsync(string status);
    }

    public class EmployerDashboardData
    {
        public int TotalJobPostings { get; set; }
        public int ActiveJobPostings { get; set; }
        public int TotalApplicants { get; set; }
        public int PendingApplications { get; set; }
        public int ShortlistedCandidates { get; set; }
        public int HiredCandidates { get; set; }
        public int UnreadNotifications { get; set; }
        public int PendingDocuments { get; set; }
    }
}

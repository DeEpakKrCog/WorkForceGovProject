using WorkForceGovProject.Models;

namespace WorkForceGovProject.Services
{
    public interface IJobOpeningService
    {
        // CRUD Operations
        Task<(bool Success, string Message, JobOpening Job)> CreateJobOpeningAsync(JobOpening jobOpening, int employerId);
        Task<JobOpening> GetJobOpeningByIdAsync(int id);
        Task<IEnumerable<JobOpening>> GetJobOpeningsByEmployerIdAsync(int employerId);
        Task<(bool Success, string Message)> UpdateJobOpeningAsync(JobOpening jobOpening, int employerId);
        Task<(bool Success, string Message)> DeleteJobOpeningAsync(int jobId, int employerId);

        // Job Status Management
        Task<(bool Success, string Message)> CloseJobOpeningAsync(int jobId, int employerId);
        Task<(bool Success, string Message)> ReopenJobOpeningAsync(int jobId, int employerId);

        // Search & Filter
        Task<IEnumerable<JobOpening>> GetActiveJobOpeningsAsync();
        Task<IEnumerable<JobOpening>> SearchJobOpeningsAsync(string searchTerm);
        Task<IEnumerable<JobOpening>> GetJobOpeningsByLocationAsync(string location);
        Task<IEnumerable<JobOpening>> GetRecentJobOpeningsAsync(int count);

        // Statistics
        Task<int> GetActiveJobsCountByEmployerAsync(int employerId);
        Task<int> GetApplicationsCountForJobAsync(int jobId);

        // Validation
        Task<bool> CanEmployerAccessJobAsync(int jobId, int employerId);
        Task<bool> IsJobOpenForApplicationsAsync(int jobId);
    }
}

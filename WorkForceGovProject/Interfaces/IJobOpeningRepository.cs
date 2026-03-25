using WorkForceGovProject.Models;

namespace WorkForceGovProject.Interfaces
{
    public interface IJobOpeningRepository
    {
        // Create
        Task<JobOpening> CreateJobOpeningAsync(JobOpening jobOpening);

        // Read
        Task<JobOpening> GetJobOpeningByIdAsync(int id);
        Task<IEnumerable<JobOpening>> GetAllJobOpeningsAsync();
        Task<IEnumerable<JobOpening>> GetJobOpeningsByEmployerIdAsync(int employerId);
        Task<IEnumerable<JobOpening>> GetActiveJobOpeningsAsync();
        Task<IEnumerable<JobOpening>> GetJobOpeningsByLocationAsync(string location);
        Task<IEnumerable<JobOpening>> GetJobOpeningsByStatusAsync(string status);
        Task<IEnumerable<JobOpening>> SearchJobOpeningsAsync(string searchTerm);

        // Update
        Task<JobOpening> UpdateJobOpeningAsync(JobOpening jobOpening);
        Task<bool> UpdateJobStatusAsync(int id, string status);
        Task<bool> CloseJobOpeningAsync(int id);

        // Delete
        Task<bool> DeleteJobOpeningAsync(int id);

        // Additional methods
        Task<bool> JobOpeningExistsAsync(int id);
        Task<int> GetActiveJobsCountByEmployerAsync(int employerId);
        Task<int> GetTotalJobOpeningsCountAsync();
        Task<IEnumerable<JobOpening>> GetRecentJobOpeningsAsync(int count);
    }
}

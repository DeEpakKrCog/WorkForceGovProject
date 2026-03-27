using WorkForceGovProject.Models;

namespace WorkForceGovProject.Interfaces
{
    /// <summary>
    /// Job Service Interface
    /// Handles job operations
    /// </summary>
    public interface IJobService
    {
        // Job Retrieval
        Task<JobOpening> GetJobByIdAsync(int id);
        Task<IEnumerable<JobOpening>> GetAllJobsAsync();
        Task<IEnumerable<JobOpening>> GetOpenJobsAsync();
        Task<IEnumerable<JobOpening>> GetRecentJobsAsync(int count);

        // Job Search
        Task<IEnumerable<JobOpening>> SearchJobsAsync(string location = "", string category = "");
        Task<IEnumerable<JobOpening>> GetJobsByLocationAsync(string location);
        Task<IEnumerable<JobOpening>> GetJobsByCategoryAsync(string category);

        // Job Creation (for employers)
        Task<(bool Success, string Message, JobOpening Job)> CreateJobAsync(JobOpening job);
        Task<(bool Success, string Message)> UpdateJobAsync(JobOpening job);
        Task<(bool Success, string Message)> CloseJobAsync(int jobId);

        // Job Statistics
        Task<int> GetJobCountAsync();
        Task<int> GetOpenJobCountAsync();
        Task<int> GetApplicationCountForJobAsync(int jobId);
    }
}

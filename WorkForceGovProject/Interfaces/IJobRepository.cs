using WorkForceGovProject.Models;

namespace WorkForceGovProject.Interfaces
{
    /// <summary>
    /// Job-specific repository with custom queries
    /// </summary>
    public interface IJobRepository : IRepository<JobOpening>
    {
        Task<JobOpening> GetJobWithApplicationsAsync(int jobId);
        Task<IEnumerable<JobOpening>> GetJobsByLocationAsync(string location);
        Task<IEnumerable<JobOpening>> GetJobsByCategoryAsync(string category);
        Task<IEnumerable<JobOpening>> GetOpenJobsAsync();
        Task<IEnumerable<JobOpening>> SearchJobsAsync(string location, string category);
        Task<IEnumerable<JobOpening>> GetRecentJobsAsync(int count);
    }
}

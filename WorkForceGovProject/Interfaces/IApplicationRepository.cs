using WorkForceGovProject.Models;

namespace WorkForceGovProject.Interfaces
{
    /// <summary>
    /// Application-specific repository with custom queries
    /// </summary>
    public interface IApplicationRepository : IRepository<Application>
    {
        Task<Application> GetApplicationWithDetailsAsync(int applicationId);
        Task<IEnumerable<Application>> GetApplicationsByCitizenAsync(int citizenId);
        Task<IEnumerable<Application>> GetApplicationsByJobAsync(int jobId);
        Task<IEnumerable<Application>> GetApplicationsByStatusAsync(string status);
        Task<Application> GetCitizenJobApplicationAsync(int citizenId, int jobId);
        Task<int> GetApplicationCountByCitizenAsync(int citizenId);
        Task<int> GetApplicationCountByJobAsync(int jobId);
        Task<IEnumerable<Application>> GetPendingApplicationsAsync();
    }
}

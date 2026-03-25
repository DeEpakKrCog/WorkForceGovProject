using WorkForceGovProject.Models;

namespace WorkForceGovProject.Interfaces
{
    public interface IApplicationRepository
    {
        // Create
        Task<Application> SubmitApplicationAsync(Application application);

        // Read
        Task<Application> GetApplicationByIdAsync(int id);
        Task<IEnumerable<Application>> GetAllApplicationsAsync();
        Task<IEnumerable<Application>> GetApplicationsByCitizenIdAsync(int citizenId);
        Task<IEnumerable<Application>> GetApplicationsByJobIdAsync(int jobId);
        Task<IEnumerable<Application>> GetApplicationsByEmployerIdAsync(int employerId);
        Task<IEnumerable<Application>> GetApplicationsByStatusAsync(string status);
        Task<Application> GetApplicationByCitizenAndJobAsync(int citizenId, int jobId);

        // Update
        Task<Application> UpdateApplicationAsync(Application application);
        Task<bool> UpdateApplicationStatusAsync(int id, string status);
        Task<bool> ShortlistApplicationAsync(int id);
        Task<bool> ApproveApplicationAsync(int id);
        Task<bool> RejectApplicationAsync(int id);

        // Delete
        Task<bool> DeleteApplicationAsync(int id);

        // Additional methods
        Task<bool> ApplicationExistsAsync(int id);
        Task<bool> HasCitizenAppliedForJobAsync(int citizenId, int jobId);
        Task<int> GetApplicationsCountByJobIdAsync(int jobId);
        Task<int> GetApplicationsCountByEmployerIdAsync(int employerId);
        Task<int> GetPendingApplicationsCountAsync(int employerId);
        Task<IEnumerable<Application>> GetShortlistedApplicationsAsync(int employerId);
    }
}

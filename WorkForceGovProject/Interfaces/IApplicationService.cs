using WorkForceGovProject.Models;

namespace WorkForceGovProject.Interfaces
{
    /// <summary>
    /// Application Service Interface
    /// Handles job application operations
    /// </summary>
    public interface IApplicationService
    {
        // Application Management
        Task<Application> GetApplicationByIdAsync(int id);
        Task<IEnumerable<Application>> GetApplicationsByCitizenAsync(int citizenId);
        Task<IEnumerable<Application>> GetApplicationsByJobAsync(int jobId);
        Task<(bool Success, string Message, Application Application)> ApplyForJobAsync(int citizenId, int jobId, string coverLetter = "");
        Task<(bool Success, string Message)> WithdrawApplicationAsync(int applicationId);

        // Application Status
        Task<Application> GetApplicationWithDetailsAsync(int applicationId);
        Task<bool> HasAppliedAsync(int citizenId, int jobId);
        Task<IEnumerable<Application>> GetApplicationsByStatusAsync(string status);

        // Application Review (for employers)
        Task<(bool Success, string Message)> ApproveApplicationAsync(int applicationId, string notes = "");
        Task<(bool Success, string Message)> RejectApplicationAsync(int applicationId, string notes = "");

        // Statistics
        Task<int> GetApplicationCountByCitizenAsync(int citizenId);
        Task<int> GetPendingApplicationCountAsync(int citizenId);
        Task<int> GetApplicationCountByJobAsync(int jobId);
    }
}

using WorkForceGovProject.Models;

namespace WorkForceGovProject.Services
{
    public interface IApplicationService
    {
        // Application Submission
        Task<(bool Success, string Message, Application Application)> SubmitApplicationAsync(int citizenId, int jobId);

        // Retrieve Applications
        Task<Application> GetApplicationByIdAsync(int id);
        Task<IEnumerable<Application>> GetApplicationsByCitizenIdAsync(int citizenId);
        Task<IEnumerable<Application>> GetApplicationsByJobIdAsync(int jobId);
        Task<IEnumerable<Application>> GetApplicationsByEmployerIdAsync(int employerId);
        Task<ApplicationDetailsDto> GetApplicationDetailsAsync(int applicationId);

        // Application Status Management
        Task<(bool Success, string Message)> ShortlistApplicationAsync(int applicationId, int employerId);
        Task<(bool Success, string Message)> ApproveApplicationAsync(int applicationId, int employerId);
        Task<(bool Success, string Message)> RejectApplicationAsync(int applicationId, int employerId);

        // Queries
        Task<IEnumerable<Application>> GetShortlistedApplicationsAsync(int employerId);
        Task<IEnumerable<Application>> GetPendingApplicationsAsync(int employerId);
        Task<int> GetPendingApplicationsCountAsync(int employerId);

        // Validation
        Task<bool> HasCitizenAppliedForJobAsync(int citizenId, int jobId);
        Task<bool> CanEmployerAccessApplicationAsync(int applicationId, int employerId);
    }

    public class ApplicationDetailsDto
    {
        public Application Application { get; set; }
        public JobOpening Job { get; set; }
        public Citizen Citizen { get; set; }
        public User CitizenUser { get; set; }
    }
}

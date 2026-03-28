using WorkForceGovProject.Models;

namespace WorkForceGovProject.Services
{
    public interface IComplianceService
    {
        Task<List<ComplianceRecord>> GetAllComplaintsAsync();
        Task<List<ComplianceRecord>> GetPendingComplaintsAsync();
        Task<List<ComplianceRecord>> GetEmployerComplianceAsync(int employerId);
        Task<(bool Success, string Message)> ReviewEmployerAsync(int employerId, string result, string notes);
        Task<(bool Success, string Message)> InvestigateComplaintAsync(int complianceId, string findings);
        Task<(bool Success, string Message)> IssueViolationAsync(int employerId, string violationType, string description);
        Task<int> GetPendingCountAsync();
        Task<int> GetViolationCountAsync();
    }
}

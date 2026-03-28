using WorkForceGovProject.Models;

namespace WorkForceGovProject.Services
{
    public interface IAuditService
    {
        Task<List<Audit>> GetAllAuditsAsync();
        Task<List<Audit>> GetPendingAuditsAsync();
        Task<List<Audit>> GetCompletedAuditsAsync();
        Task<(bool Success, string Message, Audit Audit)> CreateAuditAsync(int officerId, string scope, string findings);
        Task<(bool Success, string Message)> CompleteAuditAsync(int auditId, string finalFindings);
        Task<int> GetComplianceStatusAsync();
        Task<Dictionary<string, int>> GetAuditStatisticsAsync();
    }
}

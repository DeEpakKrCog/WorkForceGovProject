using WorkForceGovProject.Models;

namespace WorkForceGovProject.Repositories
{
    public interface IComplianceRepository : IRepository<ComplianceRecord>
    {
        Task<List<ComplianceRecord>> GetByEntityTypeAsync(string entityType);
        Task<List<ComplianceRecord>> GetByResultAsync(string result);
        Task<List<ComplianceRecord>> GetByOfficerAsync(int officerId);
        Task<List<ComplianceRecord>> GetPendingAsync();
    }
}

using WorkForceGovProject.Models;

namespace WorkForceGovProject.Interfaces
{
    /// <summary>
    /// Citizen-specific repository with custom queries
    /// </summary>
    public interface ICitizenRepository : IRepository<Citizen>
    {
        Task<Citizen> GetCitizenByUserIdAsync(int userId);
        Task<Citizen> GetCitizenWithDocumentsAsync(int citizenId);
        Task<Citizen> GetCitizenWithApplicationsAsync(int citizenId);
        Task<Citizen> GetCitizenWithBenefitsAsync(int citizenId);
        Task<IEnumerable<Citizen>> GetPendingDocumentVerificationAsync();
    }
}

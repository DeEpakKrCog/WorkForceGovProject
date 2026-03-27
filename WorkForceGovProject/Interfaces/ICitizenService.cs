using WorkForceGovProject.Models;

namespace WorkForceGovProject.Interfaces
{
    /// <summary>
    /// Citizen Service Interface
    /// Handles citizen profile operations
    /// </summary>
    public interface ICitizenService
    {
        // Profile
        Task<Citizen> GetCitizenByIdAsync(int id);
        Task<Citizen> GetCitizenByUserIdAsync(int userId);
        Task<Citizen> GetCitizenProfileAsync(int citizenId);
        Task<(bool Success, string Message)> CreateCitizenProfileAsync(int userId, string fullName, string email);
        Task<(bool Success, string Message)> UpdateCitizenProfileAsync(Citizen citizen);

        // Dashboard
        Task<(int ActiveApplications, decimal AccountBalance, string DocumentStatus, int NewJobMatches)> GetDashboardStatsAsync(int citizenId);
        Task<(bool Success, string Message)> UpdateDashboardStatsAsync(int citizenId);

        // Profile Information
        Task<(bool Success, string Message)> UpdatePersonalInfoAsync(int citizenId, string fullName, DateTime? dob, string gender, string address, string phone);
    }
}

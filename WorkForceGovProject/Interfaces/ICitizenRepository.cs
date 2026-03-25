using WorkForceGovProject.Models;

namespace WorkForceGovProject.Interfaces
{
    public interface ICitizenRepository
    {
        // Create
        Task<Citizen> CreateCitizenAsync(Citizen citizen);

        // Read
        Task<Citizen> GetCitizenByIdAsync(int id);
        Task<Citizen> GetCitizenByUserIdAsync(int userId);
        Task<IEnumerable<Citizen>> GetAllCitizensAsync();
        Task<IEnumerable<Citizen>> GetCitizensByDocumentStatusAsync(string status);

        // Update
        Task<Citizen> UpdateCitizenAsync(Citizen citizen);
        Task<bool> UpdateDocumentStatusAsync(int id, string status);
        Task<bool> UpdateAccountBalanceAsync(int id, decimal amount);
        Task<bool> IncrementActiveApplicationsAsync(int id);
        Task<bool> DecrementActiveApplicationsAsync(int id);

        // Delete
        Task<bool> DeleteCitizenAsync(int id);

        // Additional methods
        Task<bool> CitizenExistsAsync(int id);
        Task<int> GetTotalCitizensCountAsync();
    }
}

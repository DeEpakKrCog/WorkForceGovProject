using WorkForceGovProject.Models;

namespace WorkForceGovProject.Interfaces
{
    public interface IEmployerRepository
    {
        // Create
        Task<Employer> CreateEmployerAsync(Employer employer);

        // Read
        Task<Employer> GetEmployerByIdAsync(int id);
        Task<Employer> GetEmployerByUserIdAsync(int userId);
        Task<IEnumerable<Employer>> GetAllEmployersAsync();
        Task<IEnumerable<Employer>> GetEmployersByStatusAsync(string status);
        Task<IEnumerable<Employer>> GetEmployersByIndustryAsync(string industry);

        // Update
        Task<Employer> UpdateEmployerAsync(Employer employer);
        Task<bool> UpdateEmployerStatusAsync(int id, string status);

        // Delete
        Task<bool> DeleteEmployerAsync(int id);

        // Additional methods
        Task<bool> EmployerExistsAsync(int id);
        Task<bool> IsEmailRegisteredAsync(string email);
        Task<int> GetTotalEmployersCountAsync();
    }
}

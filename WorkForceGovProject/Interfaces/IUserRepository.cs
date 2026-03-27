using WorkForceGovProject.Models;

namespace WorkForceGovProject.Interfaces
{
    /// <summary>
    /// User-specific repository with custom queries
    /// </summary>
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetUserByEmailAsync(string email);
        Task<bool> UserExistsByEmailAsync(string email);
        Task<User> GetUserWithCitizenAsync(int userId);
        Task<IEnumerable<User>> GetUsersByRoleAsync(string role);
    }
}

using WorkForceGovProject.Models;

namespace WorkForceGovProject.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserWithRoleAsync(int userId);
        Task<IEnumerable<User>> GetUsersByStatusAsync(string status);
        Task<IEnumerable<User>> GetUsersByRoleAsync(int roleId);
        Task<IEnumerable<User>> SearchUsersAsync(string searchTerm);
        Task<IEnumerable<User>> GetRecentUsersAsync(int count = 10);
        Task<int> GetActiveUsersCountAsync();
        Task<int> GetInactiveUsersCountAsync();
    }
}

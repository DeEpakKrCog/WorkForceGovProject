using WorkForceGovProject.Models;

namespace WorkForceGovProject.Interfaces
{
    public interface IUserRepository
    {
        // Create
        Task<User> CreateUserAsync(User user);

        // Read
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<IEnumerable<User>> GetUsersByRoleAsync(string role);

        // Update
        Task<User> UpdateUserAsync(User user);
        Task<bool> ChangePasswordAsync(int id, string newPassword);

        // Delete
        Task<bool> DeleteUserAsync(int id);

        // Authentication
        Task<User> ValidateUserAsync(string email, string password);
        Task<bool> UserExistsAsync(string email);

        // Additional methods
        Task<bool> UserExistsByIdAsync(int id);
        Task<int> GetTotalUsersCountAsync();
    }
}

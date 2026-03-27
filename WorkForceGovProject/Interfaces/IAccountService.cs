namespace WorkForceGovProject.Interfaces
{
    /// <summary>
    /// Account Service Interface
    /// Handles user authentication and registration
    /// </summary>
    public interface IAccountService
    {
        // Authentication
        Task<(bool Success, string Message, Models.User User)> LoginAsync(string email, string password);
        Task<(bool Success, string Message)> RegisterAsync(Models.User user);
        Task<(bool Success, string Message)> LogoutAsync();

        // User Management
        Task<Models.User> GetUserByIdAsync(int userId);
        Task<Models.User> GetUserByEmailAsync(string email);
        Task<bool> UserExistsAsync(string email);
        Task<(bool Success, string Message)> UpdateUserAsync(Models.User user);
        Task<(bool Success, string Message)> ChangePasswordAsync(int userId, string oldPassword, string newPassword);

        // Validation
        Task<bool> ValidateCredentialsAsync(string email, string password);
    }
}

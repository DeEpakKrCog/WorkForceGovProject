using WorkForceGovProject.Models;

namespace WorkForceGovProject.Interfaces
{
    public interface IRoleRepository : IRepository<Role>
    {
        Task<Role> GetRoleByNameAsync(string roleName);
        Task<IEnumerable<Role>> GetRolesWithUsersCountAsync();
        Task<int> GetUsersCountByRoleAsync(int roleId);
        Task<bool> IsRoleNameUniqueAsync(string roleName, int? excludeRoleId = null);
    }
}

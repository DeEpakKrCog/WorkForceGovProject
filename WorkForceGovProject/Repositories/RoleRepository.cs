using Microsoft.EntityFrameworkCore;
using WorkForceGovProject.Interfaces;
using WorkForceGovProject.Models;
using WorkForceGovProject.Data;

namespace WorkForceGovProject.Repositories
{
    /// <summary>
    /// Role Repository - Handles all role-related database operations
    /// </summary>
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Role> GetRoleByNameAsync(string roleName)
        {
            return await _dbSet
                .FirstOrDefaultAsync(r => r.RoleName == roleName);
        }

        public async Task<IEnumerable<Role>> GetRolesWithUsersCountAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<int> GetUsersCountByRoleAsync(int roleId)
        {
            return await _context.Users
                .CountAsync(u => u.RoleId == roleId);
        }

        public async Task<bool> IsRoleNameUniqueAsync(string roleName, int? excludeRoleId = null)
        {
            var query = _dbSet.Where(r => r.RoleName == roleName);
            
            if (excludeRoleId.HasValue)
            {
                query = query.Where(r => r.RoleId != excludeRoleId.Value);
            }

            return !await query.AnyAsync();
        }
    }
}

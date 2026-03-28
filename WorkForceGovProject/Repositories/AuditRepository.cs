using WorkForceGovProject.Data;
using WorkForceGovProject.Models;
using Microsoft.EntityFrameworkCore;

namespace WorkForceGovProject.Repositories
{
    public class AuditRepository : Repository<Audit>, IAuditRepository
    {
        public AuditRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public async Task<List<Audit>> GetByOfficerAsync(int officerId)
        {
            return await _dbSet.Where(a => a.OfficerID == officerId).ToListAsync();
        }

        public async Task<List<Audit>> GetByStatusAsync(string status)
        {
            return await _dbSet.Where(a => a.Status == status).ToListAsync();
        }

        public async Task<List<Audit>> GetCompletedAsync()
        {
            return await _dbSet.Where(a => a.Status == "Completed").ToListAsync();
        }

        public async Task<List<Audit>> GetRecentAuditsAsync(DateTime startDate)
        {
            return await _dbSet.Where(a => a.Date >= startDate).ToListAsync();
        }
    }
}


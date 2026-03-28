using WorkForceGovProject.Data;
using WorkForceGovProject.Models;
using Microsoft.EntityFrameworkCore;

namespace WorkForceGovProject.Repositories
{
    public class ComplianceRepository : Repository<ComplianceRecord>, IComplianceRepository
    {
        public ComplianceRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public async Task<List<ComplianceRecord>> GetByEntityTypeAsync(string entityType)
        {
            return await _dbSet.Where(c => c.Type == entityType).ToListAsync();
        }

        public async Task<List<ComplianceRecord>> GetByResultAsync(string result)
        {
            return await _dbSet.Where(c => c.Result == result).ToListAsync();
        }

        public async Task<List<ComplianceRecord>> GetByOfficerAsync(int officerId)
        {
            // Filter compliance records by the officer who reported them (if there's such a reference)
            // For now, just return records of a specific type as a placeholder
            return await _dbSet.Take(0).ToListAsync();
        }

        public async Task<List<ComplianceRecord>> GetPendingAsync()
        {
            return await _dbSet.Where(c => c.Result == "Pending").ToListAsync();
        }
    }
}
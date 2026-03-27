using Microsoft.EntityFrameworkCore;
using WorkForceGovProject.Data;
using WorkForceGovProject.Models;
using WorkForceGovProject.Interfaces;

namespace WorkForceGovProject.Repositories.Implementations
{
    /// <summary>
    /// Job Repository Implementation
    /// Handles job opening data operations
    /// </summary>
    public class JobRepository : Repository<JobOpening>, IJobRepository
    {
        public JobRepository(ApplicationDbContext context) : base(context) { }

        public async Task<JobOpening> GetJobWithApplicationsAsync(int jobId)
        {
            return await _dbSet
                .Include(j => j.Applications)
                .FirstOrDefaultAsync(j => j.Id == jobId);
        }

        public async Task<IEnumerable<JobOpening>> GetJobsByLocationAsync(string location)
        {
            return await _dbSet
                .Where(j => j.Location.Contains(location) && j.Status == "Open")
                .OrderByDescending(j => j.PostedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<JobOpening>> GetJobsByCategoryAsync(string category)
        {
            return await _dbSet
                .Where(j => j.JobCategory == category && j.Status == "Open")
                .OrderByDescending(j => j.PostedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<JobOpening>> GetOpenJobsAsync()
        {
            return await _dbSet
                .Where(j => j.Status == "Open")
                .OrderByDescending(j => j.PostedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<JobOpening>> SearchJobsAsync(string location, string category)
        {
            var query = _dbSet.Where(j => j.Status == "Open");

            if (!string.IsNullOrEmpty(location))
                query = query.Where(j => j.Location.Contains(location));

            if (!string.IsNullOrEmpty(category))
                query = query.Where(j => j.JobCategory == category);

            return await query
                .OrderByDescending(j => j.PostedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<JobOpening>> GetRecentJobsAsync(int count)
        {
            return await _dbSet
                .Where(j => j.Status == "Open")
                .OrderByDescending(j => j.PostedDate)
                .Take(count)
                .ToListAsync();
        }
    }
}

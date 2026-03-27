using Microsoft.EntityFrameworkCore;
using WorkForceGovProject.Data;
using WorkForceGovProject.Models;
using WorkForceGovProject.Interfaces;

namespace WorkForceGovProject.Repositories.Implementations
{
    /// <summary>
    /// Application Repository Implementation
    /// Handles job application data operations
    /// </summary>
    public class ApplicationRepository : Repository<Application>, IApplicationRepository
    {
        public ApplicationRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Application> GetApplicationWithDetailsAsync(int applicationId)
        {
            return await _dbSet
                .Include(a => a.Citizen)
                .Include(a => a.JobOpening)
                .FirstOrDefaultAsync(a => a.Id == applicationId);
        }

        public async Task<IEnumerable<Application>> GetApplicationsByCitizenAsync(int citizenId)
        {
            return await _dbSet
                .Include(a => a.JobOpening)
                .Where(a => a.CitizenId == citizenId)
                .OrderByDescending(a => a.SubmittedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Application>> GetApplicationsByJobAsync(int jobId)
        {
            return await _dbSet
                .Include(a => a.Citizen)
                .Where(a => a.JobOpeningId == jobId)
                .OrderByDescending(a => a.SubmittedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Application>> GetApplicationsByStatusAsync(string status)
        {
            return await _dbSet
                .Include(a => a.Citizen)
                .Include(a => a.JobOpening)
                .Where(a => a.Status == status)
                .OrderByDescending(a => a.SubmittedDate)
                .ToListAsync();
        }

        public async Task<Application> GetCitizenJobApplicationAsync(int citizenId, int jobId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(a => a.CitizenId == citizenId && a.JobOpeningId == jobId);
        }

        public async Task<int> GetApplicationCountByCitizenAsync(int citizenId)
        {
            return await _dbSet.CountAsync(a => a.CitizenId == citizenId);
        }

        public async Task<int> GetApplicationCountByJobAsync(int jobId)
        {
            return await _dbSet.CountAsync(a => a.JobOpeningId == jobId);
        }

        public async Task<IEnumerable<Application>> GetPendingApplicationsAsync()
        {
            return await _dbSet
                .Include(a => a.Citizen)
                .Include(a => a.JobOpening)
                .Where(a => a.Status == "Pending")
                .OrderByDescending(a => a.SubmittedDate)
                .ToListAsync();
        }
    }
}

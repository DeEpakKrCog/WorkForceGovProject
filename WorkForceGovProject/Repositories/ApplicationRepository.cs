using Microsoft.EntityFrameworkCore;
using WorkForceGovProject.Data;
using WorkForceGovProject.Interfaces;
using WorkForceGovProject.Models;

namespace WorkForceGovProject.Repositories
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly ApplicationDbContext _context;

        public ApplicationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Application> SubmitApplicationAsync(Application application)
        {
            _context.Applications.Add(application);
            await _context.SaveChangesAsync();
            return application;
        }

        public async Task<Application> GetApplicationByIdAsync(int id)
        {
            return await _context.Applications.FindAsync(id);
        }

        public async Task<IEnumerable<Application>> GetAllApplicationsAsync()
        {
            return await _context.Applications
                .OrderByDescending(a => a.SubmittedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Application>> GetApplicationsByCitizenIdAsync(int citizenId)
        {
            return await _context.Applications
                .Where(a => a.CitizenId == citizenId)
                .OrderByDescending(a => a.SubmittedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Application>> GetApplicationsByJobIdAsync(int jobId)
        {
            return await _context.Applications
                .Where(a => a.JobId == jobId)
                .OrderByDescending(a => a.SubmittedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Application>> GetApplicationsByEmployerIdAsync(int employerId)
        {
            var jobIds = await _context.JobOpenings
                .Where(j => j.EmployerId == employerId)
                .Select(j => j.Id)
                .ToListAsync();

            return await _context.Applications
                .Where(a => jobIds.Contains(a.JobId))
                .OrderByDescending(a => a.SubmittedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Application>> GetApplicationsByStatusAsync(string status)
        {
            return await _context.Applications
                .Where(a => a.Status == status)
                .OrderByDescending(a => a.SubmittedDate)
                .ToListAsync();
        }

        public async Task<Application> GetApplicationByCitizenAndJobAsync(int citizenId, int jobId)
        {
            return await _context.Applications
                .FirstOrDefaultAsync(a => a.CitizenId == citizenId && a.JobId == jobId);
        }

        public async Task<Application> UpdateApplicationAsync(Application application)
        {
            _context.Applications.Update(application);
            await _context.SaveChangesAsync();
            return application;
        }

        public async Task<bool> UpdateApplicationStatusAsync(int id, string status)
        {
            var application = await GetApplicationByIdAsync(id);
            if (application == null) return false;

            application.Status = status;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ShortlistApplicationAsync(int id)
        {
            return await UpdateApplicationStatusAsync(id, "Shortlisted");
        }

        public async Task<bool> ApproveApplicationAsync(int id)
        {
            return await UpdateApplicationStatusAsync(id, "Approved");
        }

        public async Task<bool> RejectApplicationAsync(int id)
        {
            return await UpdateApplicationStatusAsync(id, "Rejected");
        }

        public async Task<bool> DeleteApplicationAsync(int id)
        {
            var application = await GetApplicationByIdAsync(id);
            if (application == null) return false;

            _context.Applications.Remove(application);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ApplicationExistsAsync(int id)
        {
            return await _context.Applications.AnyAsync(a => a.Id == id);
        }

        public async Task<bool> HasCitizenAppliedForJobAsync(int citizenId, int jobId)
        {
            return await _context.Applications
                .AnyAsync(a => a.CitizenId == citizenId && a.JobId == jobId);
        }

        public async Task<int> GetApplicationsCountByJobIdAsync(int jobId)
        {
            return await _context.Applications.CountAsync(a => a.JobId == jobId);
        }

        public async Task<int> GetApplicationsCountByEmployerIdAsync(int employerId)
        {
            var jobIds = await _context.JobOpenings
                .Where(j => j.EmployerId == employerId)
                .Select(j => j.Id)
                .ToListAsync();

            return await _context.Applications
                .CountAsync(a => jobIds.Contains(a.JobId));
        }

        public async Task<int> GetPendingApplicationsCountAsync(int employerId)
        {
            var jobIds = await _context.JobOpenings
                .Where(j => j.EmployerId == employerId)
                .Select(j => j.Id)
                .ToListAsync();

            return await _context.Applications
                .CountAsync(a => jobIds.Contains(a.JobId) && a.Status == "Pending");
        }

        public async Task<IEnumerable<Application>> GetShortlistedApplicationsAsync(int employerId)
        {
            var jobIds = await _context.JobOpenings
                .Where(j => j.EmployerId == employerId)
                .Select(j => j.Id)
                .ToListAsync();

            return await _context.Applications
                .Where(a => jobIds.Contains(a.JobId) && a.Status == "Shortlisted")
                .OrderByDescending(a => a.SubmittedDate)
                .ToListAsync();
        }
    }
}

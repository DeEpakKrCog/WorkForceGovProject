using Microsoft.EntityFrameworkCore;
using WorkForceGovProject.Data;
using WorkForceGovProject.Interfaces;
using WorkForceGovProject.Models;

namespace WorkForceGovProject.Repositories
{
    public class JobOpeningRepository : IJobOpeningRepository
    {
        private readonly ApplicationDbContext _context;

        public JobOpeningRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<JobOpening> CreateJobOpeningAsync(JobOpening jobOpening)
        {
            _context.JobOpenings.Add(jobOpening);
            await _context.SaveChangesAsync();
            return jobOpening;
        }

        public async Task<JobOpening> GetJobOpeningByIdAsync(int id)
        {
            return await _context.JobOpenings.FindAsync(id);
        }

        public async Task<IEnumerable<JobOpening>> GetAllJobOpeningsAsync()
        {
            return await _context.JobOpenings
                .OrderByDescending(j => j.PostedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<JobOpening>> GetJobOpeningsByEmployerIdAsync(int employerId)
        {
            return await _context.JobOpenings
                .Where(j => j.EmployerId == employerId)
                .OrderByDescending(j => j.PostedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<JobOpening>> GetActiveJobOpeningsAsync()
        {
            return await _context.JobOpenings
                .Where(j => j.Status == "Open")
                .OrderByDescending(j => j.PostedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<JobOpening>> GetJobOpeningsByLocationAsync(string location)
        {
            return await _context.JobOpenings
                .Where(j => j.Location.Contains(location) && j.Status == "Open")
                .OrderByDescending(j => j.PostedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<JobOpening>> GetJobOpeningsByStatusAsync(string status)
        {
            return await _context.JobOpenings
                .Where(j => j.Status == status)
                .OrderByDescending(j => j.PostedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<JobOpening>> SearchJobOpeningsAsync(string searchTerm)
        {
            return await _context.JobOpenings
                .Where(j => (j.Title.Contains(searchTerm) || 
                            j.Description.Contains(searchTerm) || 
                            j.Location.Contains(searchTerm)) && 
                            j.Status == "Open")
                .OrderByDescending(j => j.PostedDate)
                .ToListAsync();
        }

        public async Task<JobOpening> UpdateJobOpeningAsync(JobOpening jobOpening)
        {
            _context.JobOpenings.Update(jobOpening);
            await _context.SaveChangesAsync();
            return jobOpening;
        }

        public async Task<bool> UpdateJobStatusAsync(int id, string status)
        {
            var job = await GetJobOpeningByIdAsync(id);
            if (job == null) return false;

            job.Status = status;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CloseJobOpeningAsync(int id)
        {
            return await UpdateJobStatusAsync(id, "Closed");
        }

        public async Task<bool> DeleteJobOpeningAsync(int id)
        {
            var job = await GetJobOpeningByIdAsync(id);
            if (job == null) return false;

            _context.JobOpenings.Remove(job);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> JobOpeningExistsAsync(int id)
        {
            return await _context.JobOpenings.AnyAsync(j => j.Id == id);
        }

        public async Task<int> GetActiveJobsCountByEmployerAsync(int employerId)
        {
            return await _context.JobOpenings
                .CountAsync(j => j.EmployerId == employerId && j.Status == "Open");
        }

        public async Task<int> GetTotalJobOpeningsCountAsync()
        {
            return await _context.JobOpenings.CountAsync();
        }

        public async Task<IEnumerable<JobOpening>> GetRecentJobOpeningsAsync(int count)
        {
            return await _context.JobOpenings
                .Where(j => j.Status == "Open")
                .OrderByDescending(j => j.PostedDate)
                .Take(count)
                .ToListAsync();
        }
    }
}

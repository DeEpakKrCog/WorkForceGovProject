using Microsoft.EntityFrameworkCore;
using WorkForceGovProject.Data;
using WorkForceGovProject.Interfaces;
using WorkForceGovProject.Models;

namespace WorkForceGovProject.Repositories
{
    public class EmployerRepository : IEmployerRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Employer> CreateEmployerAsync(Employer employer)
        {
            _context.Employers.Add(employer);
            await _context.SaveChangesAsync();
            return employer;
        }

        public async Task<Employer> GetEmployerByIdAsync(int id)
        {
            return await _context.Employers.FindAsync(id);
        }

        public async Task<Employer> GetEmployerByUserIdAsync(int userId)
        {
            return await _context.Employers.FirstOrDefaultAsync(e => e.UserId == userId);
        }

        public async Task<IEnumerable<Employer>> GetAllEmployersAsync()
        {
            return await _context.Employers.ToListAsync();
        }

        public async Task<IEnumerable<Employer>> GetEmployersByStatusAsync(string status)
        {
            return await _context.Employers
                .Where(e => e.Status == status)
                .ToListAsync();
        }

        public async Task<IEnumerable<Employer>> GetEmployersByIndustryAsync(string industry)
        {
            return await _context.Employers
                .Where(e => e.Industry == industry)
                .ToListAsync();
        }

        public async Task<Employer> UpdateEmployerAsync(Employer employer)
        {
            _context.Employers.Update(employer);
            await _context.SaveChangesAsync();
            return employer;
        }

        public async Task<bool> UpdateEmployerStatusAsync(int id, string status)
        {
            var employer = await GetEmployerByIdAsync(id);
            if (employer == null) return false;

            employer.Status = status;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteEmployerAsync(int id)
        {
            var employer = await GetEmployerByIdAsync(id);
            if (employer == null) return false;

            _context.Employers.Remove(employer);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EmployerExistsAsync(int id)
        {
            return await _context.Employers.AnyAsync(e => e.Id == id);
        }

        public async Task<bool> IsEmailRegisteredAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return false;

            return await _context.Employers.AnyAsync(e => e.UserId == user.Id);
        }

        public async Task<int> GetTotalEmployersCountAsync()
        {
            return await _context.Employers.CountAsync();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using WorkForceGovProject.Data;
using WorkForceGovProject.Interfaces;
using WorkForceGovProject.Models;

namespace WorkForceGovProject.Repositories
{
    public class CitizenRepository : ICitizenRepository
    {
        private readonly ApplicationDbContext _context;

        public CitizenRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Citizen> CreateCitizenAsync(Citizen citizen)
        {
            _context.Citizens.Add(citizen);
            await _context.SaveChangesAsync();
            return citizen;
        }

        public async Task<Citizen> GetCitizenByIdAsync(int id)
        {
            return await _context.Citizens.FindAsync(id);
        }

        public async Task<Citizen> GetCitizenByUserIdAsync(int userId)
        {
            return await _context.Citizens.FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<IEnumerable<Citizen>> GetAllCitizensAsync()
        {
            return await _context.Citizens.ToListAsync();
        }

        public async Task<IEnumerable<Citizen>> GetCitizensByDocumentStatusAsync(string status)
        {
            return await _context.Citizens
                .Where(c => c.DocumentStatus == status)
                .ToListAsync();
        }

        public async Task<Citizen> UpdateCitizenAsync(Citizen citizen)
        {
            _context.Citizens.Update(citizen);
            await _context.SaveChangesAsync();
            return citizen;
        }

        public async Task<bool> UpdateDocumentStatusAsync(int id, string status)
        {
            var citizen = await GetCitizenByIdAsync(id);
            if (citizen == null) return false;

            citizen.DocumentStatus = status;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAccountBalanceAsync(int id, decimal amount)
        {
            var citizen = await GetCitizenByIdAsync(id);
            if (citizen == null) return false;

            citizen.AccountBalance = amount;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IncrementActiveApplicationsAsync(int id)
        {
            var citizen = await GetCitizenByIdAsync(id);
            if (citizen == null) return false;

            citizen.ActiveApplications++;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DecrementActiveApplicationsAsync(int id)
        {
            var citizen = await GetCitizenByIdAsync(id);
            if (citizen == null) return false;

            if (citizen.ActiveApplications > 0)
            {
                citizen.ActiveApplications--;
                await _context.SaveChangesAsync();
            }
            return true;
        }

        public async Task<bool> DeleteCitizenAsync(int id)
        {
            var citizen = await GetCitizenByIdAsync(id);
            if (citizen == null) return false;

            _context.Citizens.Remove(citizen);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CitizenExistsAsync(int id)
        {
            return await _context.Citizens.AnyAsync(c => c.Id == id);
        }

        public async Task<int> GetTotalCitizensCountAsync()
        {
            return await _context.Citizens.CountAsync();
        }
    }
}

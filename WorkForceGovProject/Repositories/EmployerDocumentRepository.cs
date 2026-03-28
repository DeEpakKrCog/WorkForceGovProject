using Microsoft.EntityFrameworkCore;
using WorkForceGovProject.Data;
using WorkForceGovProject.Interfaces;
using WorkForceGovProject.Models;

namespace WorkForceGovProject.Repositories
{
    public class EmployerDocumentRepository : IEmployerDocumentRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployerDocumentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<EmployerDocument> UploadDocumentAsync(EmployerDocument document)
        {
            _context.EmployerDocuments.Add(document);
            await _context.SaveChangesAsync();
            return document;
        }

        public async Task<EmployerDocument> GetDocumentByIdAsync(int id)
        {
            return await _context.EmployerDocuments.FindAsync(id);
        }

        public async Task<IEnumerable<EmployerDocument>> GetDocumentsByEmployerIdAsync(int employerId)
        {
            return await _context.EmployerDocuments
                .Where(d => d.EmployerId == employerId)
                .OrderByDescending(d => d.UploadedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<EmployerDocument>> GetDocumentsByStatusAsync(string status)
        {
            return await _context.EmployerDocuments
                .Where(d => d.VerificationStatus == status)
                .OrderByDescending(d => d.UploadedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<EmployerDocument>> GetDocumentsByTypeAsync(string docType)
        {
            return await _context.EmployerDocuments
                .Where(d => d.DocType == docType)
                .OrderByDescending(d => d.UploadedDate)
                .ToListAsync();
        }

        public async Task<EmployerDocument> UpdateDocumentAsync(EmployerDocument document)
        {
            _context.EmployerDocuments.Update(document);
            await _context.SaveChangesAsync();
            return document;
        }

        public async Task<bool> UpdateVerificationStatusAsync(int id, string status)
        {
            var document = await GetDocumentByIdAsync(id);
            if (document == null) return false;

            document.VerificationStatus = status;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteDocumentAsync(int id)
        {
            var document = await GetDocumentByIdAsync(id);
            if (document == null) return false;

            _context.EmployerDocuments.Remove(document);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DocumentExistsAsync(int id)
        {
            return await _context.EmployerDocuments.AnyAsync(d => d.Id == id);
        }

        public async Task<int> GetPendingDocumentsCountAsync()
        {
            return await _context.EmployerDocuments
                .CountAsync(d => d.VerificationStatus == "Pending");
        }

        public async Task<IEnumerable<EmployerDocument>> GetPendingVerificationDocumentsAsync()
        {
            return await _context.EmployerDocuments
                .Where(d => d.VerificationStatus == "Pending")
                .OrderBy(d => d.UploadedDate)
                .ToListAsync();
        }
    }
}

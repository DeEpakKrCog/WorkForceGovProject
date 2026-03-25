using WorkForceGovProject.Models;

namespace WorkForceGovProject.Interfaces
{
    public interface IEmployerDocumentRepository
    {
        // Create
        Task<EmployerDocument> UploadDocumentAsync(EmployerDocument document);

        // Read
        Task<EmployerDocument> GetDocumentByIdAsync(int id);
        Task<IEnumerable<EmployerDocument>> GetDocumentsByEmployerIdAsync(int employerId);
        Task<IEnumerable<EmployerDocument>> GetDocumentsByStatusAsync(string status);
        Task<IEnumerable<EmployerDocument>> GetDocumentsByTypeAsync(string docType);

        // Update
        Task<EmployerDocument> UpdateDocumentAsync(EmployerDocument document);
        Task<bool> UpdateVerificationStatusAsync(int id, string status);

        // Delete
        Task<bool> DeleteDocumentAsync(int id);

        // Additional methods
        Task<bool> DocumentExistsAsync(int id);
        Task<int> GetPendingDocumentsCountAsync();
        Task<IEnumerable<EmployerDocument>> GetPendingVerificationDocumentsAsync();
    }
}

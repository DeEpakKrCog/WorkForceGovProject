using WorkForceGovProject.Models;

namespace WorkForceGovProject.Services
{
    public interface IEmployerDocumentService
    {
        // Document Management
        Task<(bool Success, string Message, EmployerDocument Document)> UploadDocumentAsync(EmployerDocument document, int employerId);
        Task<IEnumerable<EmployerDocument>> GetDocumentsByEmployerIdAsync(int employerId);
        Task<EmployerDocument> GetDocumentByIdAsync(int id);

        // Verification (for Admin/Labor Officer)
        Task<(bool Success, string Message)> VerifyDocumentAsync(int documentId, string status);
        Task<IEnumerable<EmployerDocument>> GetPendingVerificationDocumentsAsync();

        // Validation
        Task<bool> CanEmployerAccessDocumentAsync(int documentId, int employerId);
        Task<bool> HasPendingDocumentsAsync(int employerId);
    }
}

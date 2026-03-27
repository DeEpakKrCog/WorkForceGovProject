using WorkForceGovProject.Models;

namespace WorkForceGovProject.Interfaces
{
    /// <summary>
    /// Document Service Interface
    /// Handles document upload and verification
    /// </summary>
    public interface IDocumentService
    {
        // Document Management
        Task<CitizenDocument> GetDocumentByIdAsync(int id);
        Task<IEnumerable<CitizenDocument>> GetCitizenDocumentsAsync(int citizenId);
        Task<(bool Success, string Message, CitizenDocument Document)> UploadDocumentAsync(
            int citizenId, string documentType, IFormFile file);
        Task<(bool Success, string Message)> DeleteDocumentAsync(int documentId);

        // Verification
        Task<(bool Success, string Message)> VerifyDocumentAsync(int documentId);
        Task<(bool Success, string Message)> RejectDocumentAsync(int documentId, string reason);
        Task<IEnumerable<CitizenDocument>> GetPendingVerificationAsync();

        // Status
        Task<string> GetDocumentVerificationStatusAsync(int citizenId);
        Task<int> GetDocumentCountAsync(int citizenId);
        Task<int> GetVerifiedDocumentCountAsync(int citizenId);

        // File Operations
        Task<string> SaveFileAsync(IFormFile file, int citizenId, string documentType);
        Task<bool> DeleteFileAsync(string filePath);
        Task<byte[]> GetFileAsync(string filePath);
    }
}

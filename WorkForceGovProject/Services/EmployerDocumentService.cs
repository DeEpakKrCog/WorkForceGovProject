using WorkForceGovProject.Interfaces;
using WorkForceGovProject.Models;

namespace WorkForceGovProject.Services
{
    public class EmployerDocumentService : IEmployerDocumentService
    {
        private readonly IEmployerDocumentRepository _employerDocumentRepository;
        private readonly IEmployerRepository _employerRepository;
        private readonly INotificationService _notificationService;

        public EmployerDocumentService(
            IEmployerDocumentRepository employerDocumentRepository,
            IEmployerRepository employerRepository,
            INotificationService notificationService)
        {
            _employerDocumentRepository = employerDocumentRepository;
            _employerRepository = employerRepository;
            _notificationService = notificationService;
        }

        public async Task<(bool Success, string Message, EmployerDocument Document)> UploadDocumentAsync(EmployerDocument document, int employerId)
        {
            // Validate employer exists
            var employer = await _employerRepository.GetEmployerByIdAsync(employerId);
            if (employer == null)
            {
                return (false, "Employer not found.", null);
            }

            // Validate document data
            if (string.IsNullOrWhiteSpace(document.DocType))
            {
                return (false, "Document type is required.", null);
            }

            if (string.IsNullOrWhiteSpace(document.FileURL))
            {
                return (false, "Document file is required.", null);
            }

            // Set document properties
            document.EmployerId = employerId;
            document.UploadedDate = DateTime.Now;
            document.VerificationStatus = "Pending";

            var uploadedDocument = await _employerDocumentRepository.UploadDocumentAsync(document);

            return (true, "Document uploaded successfully! Waiting for verification.", uploadedDocument);
        }

        public async Task<IEnumerable<EmployerDocument>> GetDocumentsByEmployerIdAsync(int employerId)
        {
            return await _employerDocumentRepository.GetDocumentsByEmployerIdAsync(employerId);
        }

        public async Task<EmployerDocument> GetDocumentByIdAsync(int id)
        {
            return await _employerDocumentRepository.GetDocumentByIdAsync(id);
        }

        public async Task<(bool Success, string Message)> VerifyDocumentAsync(int documentId, string status)
        {
            var document = await _employerDocumentRepository.GetDocumentByIdAsync(documentId);
            if (document == null)
            {
                return (false, "Document not found.");
            }

            if (status != "Verified" && status != "Rejected")
            {
                return (false, "Invalid verification status.");
            }

            await _employerDocumentRepository.UpdateVerificationStatusAsync(documentId, status);

            // Notify employer
            await _notificationService.CreateDocumentVerificationNotificationAsync(document.EmployerId, status);

            // If all documents verified, update employer status
            if (status == "Verified")
            {
                var allDocuments = await _employerDocumentRepository.GetDocumentsByEmployerIdAsync(document.EmployerId);
                var allVerified = allDocuments.All(d => d.VerificationStatus == "Verified");

                if (allVerified)
                {
                    var employer = await _employerRepository.GetEmployerByIdAsync(document.EmployerId);
                    if (employer != null)
                    {
                        employer.Status = "Verified";
                        await _employerRepository.UpdateEmployerAsync(employer);
                    }
                }
            }

            return (true, $"Document {status.ToLower()} successfully!");
        }

        public async Task<IEnumerable<EmployerDocument>> GetPendingVerificationDocumentsAsync()
        {
            return await _employerDocumentRepository.GetPendingVerificationDocumentsAsync();
        }

        public async Task<bool> CanEmployerAccessDocumentAsync(int documentId, int employerId)
        {
            var document = await _employerDocumentRepository.GetDocumentByIdAsync(documentId);
            return document?.EmployerId == employerId;
        }

        public async Task<bool> HasPendingDocumentsAsync(int employerId)
        {
            var documents = await _employerDocumentRepository.GetDocumentsByEmployerIdAsync(employerId);
            return documents.Any(d => d.VerificationStatus == "Pending");
        }
    }
}

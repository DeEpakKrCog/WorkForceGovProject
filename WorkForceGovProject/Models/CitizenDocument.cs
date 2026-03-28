using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkForceGovProject.Models
{
    public class CitizenDocument
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CitizenId { get; set; }

        [ForeignKey("CitizenId")]
        public virtual Citizen Citizen { get; set; }

        [Required]
        public string DocumentType { get; set; } // Identity, Qualification, Resume, etc.

        [Required]
        public string FileName { get; set; }

        public string FilePath { get; set; } // URL or file path

        [DataType(DataType.Date)]
        public DateTime UploadedDate { get; set; } = DateTime.Now;

        [Required]
        public string VerificationStatus { get; set; } = "Pending"; // Pending, Verified, Rejected

        public string RejectionReason { get; set; }

        [DataType(DataType.Date)]
        public DateTime? VerificationDate { get; set; }

        // Compatibility aliases for other branch patterns
        [NotMapped]
        public int DocumentID { get => Id; set => Id = value; }

        [NotMapped]
        public int CitizenID { get => CitizenId; set => CitizenId = value; }

        [NotMapped]
        public string DocType { get => DocumentType; set => DocumentType = value; }

        [NotMapped]
        public string FileURI { get => FilePath; set => FilePath = value; }

        [NotMapped]
        public string VerificationNotes { get => RejectionReason; set => RejectionReason = value; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkForceGovProject.Models
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [Required]
        public string Message { get; set; }

        public string Category { get; set; } // JobApplication, DocumentVerification, ProgramEnrollment, BenefitApproval

        public int? EntityId { get; set; } // ID of the related entity (Application, Document, etc.)

        public string EntityType { get; set; } // Application, Document, Benefit, etc.

        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public bool IsRead { get; set; } = false;

        public string Status { get; set; } = "Active"; // Active, Archived
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkForceGovProject.Models
{
    public class Application
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CitizenId { get; set; }

        [ForeignKey("CitizenId")]
        public virtual Citizen Citizen { get; set; }

        [Required]
        public int JobOpeningId { get; set; }

        [ForeignKey("JobOpeningId")]
        public virtual JobOpening JobOpening { get; set; }

        [NotMapped]
        public int JobId
        {
            get => JobOpeningId;
            set => JobOpeningId = value;
        }

        [DataType(DataType.Date)]
        public DateTime SubmittedDate { get; set; } = DateTime.Now;

        [Required]
        public string Status { get; set; } = "Pending"; // Pending, Under Review, Approved, Rejected, Withdrawn

        public string CoverLetter { get; set; }

        public DateTime? ReviewedDate { get; set; }

        public string ReviewNotes { get; set; }

        // Compatibility aliases for other branch patterns
        [NotMapped]
        public DateTime ApplicationDate { get => SubmittedDate; set => SubmittedDate = value; }

        [NotMapped]
        public string ReviewedBy { get; set; }

        [NotMapped]
        public string OfficerNotes { get; set; }
    }
}

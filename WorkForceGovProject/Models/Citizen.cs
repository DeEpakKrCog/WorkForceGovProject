using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkForceGovProject.Models
{
    public class Citizen
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        // --- Profile Fields from Documentation ---
        [DataType(DataType.Date)]
        public DateTime? DOB { get; set; }

        public string? Gender { get; set; }

        public string? Address { get; set; }

        public string? PhoneNumber { get; set; }

        // --- Dashboard Stats ---
        public int ActiveApplications { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal AccountBalance { get; set; } = 0.00m;

        public string DocumentStatus { get; set; } = "Pending";

        public int NewJobMatches { get; set; } = 0;

        // --- Link to User Account ---
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        // --- Navigation Properties ---
        public virtual ICollection<CitizenDocument> Documents { get; set; } = new List<CitizenDocument>();
        public virtual ICollection<Application> Applications { get; set; } = new List<Application>();
        public virtual ICollection<Benefit> Benefits { get; set; } = new List<Benefit>();

        // Compatibility alias for ProgramManager branch
        [NotMapped]
        public int CitizenID { get => Id; set => Id = value; }
    }
}

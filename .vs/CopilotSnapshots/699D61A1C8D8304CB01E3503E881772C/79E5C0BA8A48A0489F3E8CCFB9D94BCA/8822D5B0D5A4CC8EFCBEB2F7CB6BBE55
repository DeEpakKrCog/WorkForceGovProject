using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkForceGovProject.Models
{
    public class Citizen
    {
        [Key] // Required: Primary Key for the Database
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public int ActiveApplications { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")] // Required for currency precision
        public decimal AccountBalance { get; set; } = 0.00m;

        public string DocumentStatus { get; set; } = "Pending";

        public int NewJobMatches { get; set; } = 0;

        // Relationship to the User table (Foreign Key)
        // This links the Citizen details to their Login account
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
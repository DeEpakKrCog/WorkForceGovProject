using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkForceGovProject.Models
{
    public class JobApplication
    {
        [Key]
        public int ApplicationID { get; set; }

        public int CitizenID { get; set; }

        [ForeignKey("CitizenID")]
        public virtual Citizen Citizen { get; set; }

        [Required]
        public string JobTitle { get; set; }

        [Required]
        public string EmployerName { get; set; }

        public string EmployerCompany { get; set; }

        public DateTime ApplicationDate { get; set; }

        [Required]
        public string Status { get; set; } // "Pending", "Approved", "Rejected", "UnderReview"

        public string OfficerNotes { get; set; }

        public DateTime? ReviewedDate { get; set; }

        public string ReviewedBy { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkForceGovProject.Models
{
    public class JobOpening
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string JobTitle { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int EmployerId { get; set; }

        [Required]
        public string Location { get; set; }

        public string JobCategory { get; set; } // IT, Finance, Manufacturing, etc.

        [Column(TypeName = "decimal(18,2)")]
        public decimal SalaryMin { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal SalaryMax { get; set; }

        [DataType(DataType.Date)]
        public DateTime PostedDate { get; set; } = DateTime.Now;

        [DataType(DataType.Date)]
        public DateTime? ClosingDate { get; set; }

        public string Status { get; set; } = "Open"; // Open, Closed, On Hold

        public int TotalApplications { get; set; } = 0;

        public virtual ICollection<Application> Applications { get; set; } = new List<Application>();
    }
}

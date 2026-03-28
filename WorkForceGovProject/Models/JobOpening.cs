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

        // Backwards-compatible properties expected by employer branch
        [NotMapped]
        public string Title
        {
            get => JobTitle;
            set => JobTitle = value;
        }

        [NotMapped]
        public decimal Salary
        {
            get => SalaryMax; 
            set { SalaryMin = value; SalaryMax = value; }
        }

        [NotMapped]
        public string Requirements { get; set; }

        [NotMapped]
        public DateTime? ApplicationDeadline { get; set; }

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

        public DateTime? ClosingDate { get; set; }

        public string Status { get; set; } = "Open"; // Open, Closed, On Hold

        public int TotalApplications { get; set; } = 0;

        public virtual ICollection<Application> Applications { get; set; } = new List<Application>();
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkForceGovProject.Models
{
    public class Resource
    {
        [Key] // Primary Key
        public int ResourceID { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Resource Type")]
        public string Type { get; set; }

        [Required]
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Available";

        // Foreign Key relationship to EmploymentProgram
        [Required]
        public int ProgramID { get; set; }

        [ForeignKey("ProgramID")]
        public virtual EmploymentProgram? EmploymentProgram { get; set; }
    }
}

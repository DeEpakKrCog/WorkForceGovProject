using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkForceGovProject.Models
{
    public class Training
    {
        [Key] // Primary Key
        public int TrainingID { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Active";

        // Foreign Key relationship to EmploymentProgram
        [Required]
        public int ProgramID { get; set; }

        [ForeignKey("ProgramID")]
        public virtual EmploymentProgram? EmploymentProgram { get; set; }
    }
}

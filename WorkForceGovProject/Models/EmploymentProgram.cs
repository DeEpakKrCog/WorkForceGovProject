using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkForceGovProject.Models
{
    public class EmploymentProgram
    {
        [Key] // Primary Key
        public int ProgramID { get; set; }

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
        [Column(TypeName = "decimal(18,2)")] // Currency precision
        [Display(Name = "Program Budget")]
        public decimal Budget { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Active";

        // Navigation properties for One-to-Many relationships
        public virtual ICollection<Training>? Trainings { get; set; }
        public virtual ICollection<Resource>? Resources { get; set; }
        public virtual ICollection<Benefit>? Benefits { get; set; }
    }
}

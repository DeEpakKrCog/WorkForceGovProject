using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkForceGovProject.Models
{
    public class Benefit
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProgramId { get; set; }

        [ForeignKey("ProgramId")]
        public virtual EmploymentProgram Program { get; set; }

        [Required]
        public int CitizenId { get; set; }

        [ForeignKey("CitizenId")]
        public virtual Citizen Citizen { get; set; }

        [Required]
        public string BenefitType { get; set; } // Cash, Subsidy, Training

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [DataType(DataType.Date)]
        public DateTime BenefitDate { get; set; } = DateTime.Now;

        public string Status { get; set; } = "Active"; // Active, Completed, Withdrawn

        public string Description { get; set; }
    }
}

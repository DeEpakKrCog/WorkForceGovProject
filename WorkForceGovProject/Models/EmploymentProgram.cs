using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkForceGovProject.Models
{
    public class EmploymentProgram
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ProgramName { get; set; }

        public string Description { get; set; }

        public string ProgramType { get; set; } // Cash, Subsidy, Training

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalBudget { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        public string Status { get; set; } = "Active"; // Active, Closed, Paused

        public virtual ICollection<Benefit> Benefits { get; set; } = new List<Benefit>();

        // Compatibility aliases for ProgramManager branch
        [NotMapped]
        public int ProgramID { get => Id; set => Id = value; }

        [NotMapped]
        public string Title { get => ProgramName; set => ProgramName = value; }

        [NotMapped]
        public decimal Budget { get => TotalBudget; set => TotalBudget = value; }

        [NotMapped]
        public DateTime? ApplicationEndDate { get => EndDate; set => EndDate = value; }

        [NotMapped]
        public ICollection<Training> Trainings { get; set; } = new List<Training>();

        [NotMapped]
        public ICollection<Resource> Resources { get; set; } = new List<Resource>();
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkForceGovProject.Models
{
    public class Report
    {
        [Key]
        public int ReportId { get; set; }

        [Required]
        [StringLength(100)]
        public string ReportName { get; set; }

        [Required]
        public int GeneratedBy { get; set; } // Admin ID

        [DataType(DataType.DateTime)]
        public DateTime GeneratedDate { get; set; } = DateTime.Now;

        [Required]
        [StringLength(50)]
        public string ReportType { get; set; } // Employment, Compliance, Participation, etc.

        [Column(TypeName = "nvarchar(max)")]
        public string ReportContent { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? EndDate { get; set; }
    }
}

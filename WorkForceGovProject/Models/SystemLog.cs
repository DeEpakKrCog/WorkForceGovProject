using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkForceGovProject.Models
{
    public class SystemLog
    {
        [Key]
        public int LogId { get; set; }

        [Required]
        public int UserId { get; set; }

        [StringLength(255)]
        public string Action { get; set; }

        [StringLength(100)]
        public string Resource { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Timestamp { get; set; } = DateTime.Now;

        [StringLength(50)]
        public string IpAddress { get; set; }

        // Foreign key to Admin
        public int? AdminId { get; set; }
        [ForeignKey("AdminId")]
        public virtual Admin Admin { get; set; }
    }
}

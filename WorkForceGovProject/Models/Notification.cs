using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkForceGovProject.Models
{
    public class Notification
    {
        [Key]
        public int NotificationID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        [StringLength(500)]
        public string Message { get; set; }

        [Required]
        [StringLength(50)]
        public string Category { get; set; } // "Program Update", "Budget Alert", "Training Schedule", "Performance Alert"

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Unread"; // "Read" or "Unread"

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation property
        [ForeignKey("UserID")]
        public virtual User? User { get; set; }
    }
}

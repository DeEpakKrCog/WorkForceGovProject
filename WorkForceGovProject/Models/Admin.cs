using System.ComponentModel.DataAnnotations;

namespace WorkForceGovProject.Models
{
    public class Admin
    {
        [Key]
        public int AdminId { get; set; }

        [Required(ErrorMessage = "Admin name is required")]
        [StringLength(100)]
        public string AdminName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(255)]
        public string Password { get; set; }

        [Phone]
        [StringLength(15)]
        public string Phone { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public bool IsActive { get; set; } = true;

        // Relationship to SystemLogs
        public virtual ICollection<SystemLog> SystemLogs { get; set; } = new List<SystemLog>();
    }
}

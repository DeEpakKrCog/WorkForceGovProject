using System.ComponentModel.DataAnnotations;

namespace WorkForceGovProject.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }

        [Required(ErrorMessage = "Role name is required")]
        [StringLength(50)]
        public string RoleName { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        // Relationship to User
        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}

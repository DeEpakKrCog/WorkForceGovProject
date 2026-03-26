using System.ComponentModel.DataAnnotations;

namespace WorkForceGovProject.Models.ViewModels
{
    public class UserManagementViewModel
    {
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Role { get; set; }

        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    public class CreateUserViewModel
    {
        [Required(ErrorMessage = "Full Name is required")]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(255, MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Role { get; set; }

        public int? RoleId { get; set; }
    }

    public class EditUserViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Role { get; set; }

        public int? RoleId { get; set; }

        [Required]
        public string Status { get; set; }
    }

    public class RoleManagementViewModel
    {
        public int RoleId { get; set; }

        [Required]
        [StringLength(50)]
        public string RoleName { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public int UserCount { get; set; }
    }

    public class CreateRoleViewModel
    {
        [Required(ErrorMessage = "Role name is required")]
        [StringLength(50)]
        public string RoleName { get; set; }

        [StringLength(500)]
        public string Description { get; set; }
    }
}

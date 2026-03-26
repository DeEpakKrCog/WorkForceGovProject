using System.ComponentModel.DataAnnotations;

namespace WorkForceGovProject.Models.ViewModels
{
    public class AdminLoginViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }

    public class AdminRegisterViewModel
    {
        [Required(ErrorMessage = "Admin Name is required")]
        [StringLength(100)]
        public string AdminName { get; set; }

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

        [Phone]
        [StringLength(15)]
        public string Phone { get; set; }
    }

    public class AdminEditViewModel
    {
        public int AdminId { get; set; }

        [Required]
        [StringLength(100)]
        public string AdminName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        [StringLength(15)]
        public string Phone { get; set; }

        public bool IsActive { get; set; }
    }

    public class AdminDashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int InactiveUsers { get; set; }
        public int TotalRoles { get; set; }
        public int RecentLogsCount { get; set; }
        public List<SystemLog> RecentLogs { get; set; } = new List<SystemLog>();
        public List<User> RecentUsers { get; set; } = new List<User>();
    }
}

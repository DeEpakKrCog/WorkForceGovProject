using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkForceGovProject.Models
{
    public class Employer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string CompanyName { get; set; }

        [Required]
        [StringLength(100)]
        public string Industry { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [StringLength(100)]
        public string ContactInfo { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Pending"; // Active / Verified / Pending

        public int UserId { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace WorkForceGovProject.Models
{
    public class Audit
    {
        [Key]
        public int AuditID { get; set; }
       
        public int OfficerID { get; set; } // FK to User Table [cite: 122]

        public string Scope { get; set; } // "Application", "Employer", or "Program" [cite: 122]

        public string Findings { get; set; }
       

        public DateTime Date { get; set; }
        public string Status { get; set; } // "Open", "Closed", "Pending" [cite: 122]
    }
}
using System.ComponentModel.DataAnnotations;

namespace WorkForceGovProject.Models
{
    public class ComplianceRecord
    {
        [Key]
        public int ComplianceID { get; set; }
        

        public int EntityID { get; set; } // Linked to Employer, Application, or Program [cite: 117]

        public string Type { get; set; } // "Employer", "Application", or "Program" [cite: 117]

        public string Result { get; set; } // "Compliant", "Non-Compliant", "Under Review" [cite: 117]

        public DateTime Date { get; set; }
        
        public string Notes { get; set; }
   
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkForceGovProject.Models
{
    public class CitizenDocument
    {
        [Key]
        public int DocumentID { get; set; }
        

        public int CitizenID { get; set; }
        

        [ForeignKey("CitizenID")]
        public virtual Citizen Citizen { get; set; }
        

        [Required]
        public string DocType { get; set; } // e.g., "IDProof" or "Qualification" [cite: 105]

        public string FileURI { get; set; }
       

        public DateTime UploadedDate { get; set; }
        

        public string VerificationStatus { get; set; } // "Verified", "Pending", "Rejected" [cite: 105]

        public string VerificationNotes { get; set; }
       
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ST10357066_PROG6212_CMCS_Part1.Models
{
    public class Approval
    {
        [Key]
        public int ApprovalID { get; set; }

        [ForeignKey("Claim")]
        public int ClaimID { get; set; } // Foreign key for Claim
        public virtual Claim Claim { get; set; }  // Navigation property for Claim

        [ForeignKey("ApprovedByUser")]
        public int UserID { get; set; } // Foreign key for User
        public virtual User ApprovedByUser { get; set; }  // Navigation property for User

        [Required]
        public DateTime ApprovalDate { get; set; } = DateTime.Now;  // Default value set to current date

        [Required]
        [StringLength(50)]
        public string? Status { get; set; }  // Example: "Approved" or "Rejected"

        public string? Comments { get; set; }  // Optional comments

        // Constructor to initialize the collection
        public Approval()
        {
        }
    }
}

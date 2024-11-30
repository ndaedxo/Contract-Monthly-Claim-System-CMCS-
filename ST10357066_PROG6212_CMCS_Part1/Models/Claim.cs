using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;
using System.Runtime.ConstrainedExecution;

namespace ST10357066_PROG6212_CMCS_Part1.Models
{
    // 3. Claims Table
    public class Claim
    {
        public int ClaimID { get; set; }
        public int UserID { get; set; }
        public DateTime ClaimDate { get; set; }   // Default to the current date and time


        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Hours { get; set; } = 3;

        [Column(TypeName = "decimal(18,2)")]
        public decimal hourlyRate { get; set; } = 3;
        public string ValidationError { get; set; } = string.Empty;
        public string Status { get; set; }  // Default status
        public string Description { get; set; }  // Default to an empty string
        public DateTime CreatedAt { get; set; }  // Default to the current date and time
        public DateTime UpdatedAt { get; set; }  // Default to the current date and time

        // Navigation property for the User
        public User User { get; set; }

        // Navigation property for the Documents
        public ICollection<Document> Documents { get; set; }  // Initialize as an empty list


        public Claim()
        {
            ClaimDate = DateTime.Now;
            Amount = 0.0m;
            Description = string.Empty;
            Status = "Pending";
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;

            Documents = new List<Document>();
        }


        public Claim(int userID, decimal amount, string description = "")
        {
            UserID = userID;
            Amount = amount;
            Description = description;
            Status = "Pending";
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            ClaimDate = DateTime.Now;

            Documents = new List<Document>();
        }

        public override string ToString()
        {
            return $"ClaimID: {ClaimID}, UserID: {UserID}, Amount: {Amount}, Status: {Status}, CreatedAt: {CreatedAt}, UpdatedAt: {UpdatedAt}";
        }

    }
}

using System.Security.Claims;

namespace ST10357066_PROG6212_CMCS_Part1.Models
{
    // 4. Documents Table
    public class Document
    {
        public int DocumentID { get; set; }
        public int ClaimID { get; set; }
        public string DocumentPath { get; set; }
        public string FileName { get; set; }
        public DateTime UploadDate { get; set; }  
        public string Description { get; set; }  

        // Navigation property for the Claim
        public Claim Claim { get; set; }


        public Document()
        {
            FileName = string.Empty;
            DocumentPath = string.Empty;
            Description = string.Empty;
            UploadDate = DateTime.Now;

        }


        public Document(int claimID, string documentPath, string description = "")
        {
            FileName = string.Empty;
            ClaimID = claimID;
            DocumentPath = documentPath;
            Description = description;
            UploadDate = DateTime.Now;
        }

        public override string ToString()
        {
            return $"DocumentID: {DocumentID}, ClaimID: {ClaimID}, DocumentPath: {DocumentPath}, Description: {Description}, UploadDate: {UploadDate}";
        }

    }
}

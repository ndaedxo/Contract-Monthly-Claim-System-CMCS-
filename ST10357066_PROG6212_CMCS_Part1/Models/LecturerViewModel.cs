using System;
using System.Collections.Generic;
using System.Linq;

namespace ST10357066_PROG6212_CMCS_Part1.Models
{
    public class LecturerViewModel
    {
        // Properties to store lecturer details
        public int LecturerID { get; set; }
        public string? LecturerName { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;

        // Collection of Claims associated with the lecturer
        public ICollection<Claim> Claims { get; set; } = new List<Claim>();

        public LecturerViewModel() { }

        // Constructor to initialize the lecturer
        public LecturerViewModel(int lecturerID, string lecturerName, string email)
        {
            LecturerID = lecturerID;
            LecturerName = lecturerName;
            Email = email;
        }

        // Method for lecturers to submit claims
        public void SubmitClaim(decimal hoursWorked, decimal hourlyRate, string description = "")
        {
            decimal amount = hoursWorked * hourlyRate;
            var newClaim = new Claim(LecturerID, amount, description);
            Claims.Add(newClaim);

            Console.WriteLine($"Claim submitted: {newClaim}");
        }

        // Method to allow lecturers to upload supporting documents for claims
        public void UploadDocument(int claimID, string documentPath, string description = "")
        {
            var claim = Claims.FirstOrDefault(c => c.ClaimID == claimID);
            if (claim != null)
            {
                var newDocument = new Document(claimID, documentPath, description);
                claim.Documents.Add(newDocument);

                Console.WriteLine($"Document uploaded for claim {claimID}: {newDocument}");
            }
            else
            {
                Console.WriteLine($"Claim with ID {claimID} not found.");
            }
        }

        // Method to update the status of a claim
        public void UpdateClaimStatus(int claimID, string status)
        {
            var claim = Claims.FirstOrDefault(c => c.ClaimID == claimID);
            if (claim != null)
            {
                claim.Status = status;
                claim.UpdatedAt = DateTime.Now;

                Console.WriteLine($"Claim {claimID} status updated to: {status}");
            }
            else
            {
                Console.WriteLine($"Claim with ID {claimID} not found.");
            }
        }

        // Method to track claims and display status updates
        public void TrackClaimStatus(int claimID)
        {
            var claim = Claims.FirstOrDefault(c => c.ClaimID == claimID);
            if (claim != null)
            {
                Console.WriteLine($"Claim {claimID} is currently: {claim.Status}");
            }
            else
            {
                Console.WriteLine($"Claim with ID {claimID} not found.");
            }
        }

        public override string ToString()
        {
            return $"LecturerID: {LecturerID}, LecturerName: {LecturerName}, Email: {Email}, Claims Count: {Claims.Count}";
        }
    }
}

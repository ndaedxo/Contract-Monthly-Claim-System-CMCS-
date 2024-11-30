using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ST10357066_PROG6212_CMCS_Part1.Data;
using ST10357066_PROG6212_CMCS_Part1.Models;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace ST10357066_PROG6212_CMCS_Part1.Controllers
{
    [Authorize]
    public class LecturerDashboardController : Controller
    {
        private readonly CMCSDbContext _context;
        private readonly ILogger<LecturerDashboardController> _logger;
        private readonly IHubContext<ClaimsHub> _hubContext;

        public LecturerDashboardController(
            CMCSDbContext context,
            ILogger<LecturerDashboardController> logger,
            IHubContext<ClaimsHub> hubContext)
        {
            _context = context;
            _logger = logger;
            _hubContext = hubContext;
        }

        // Fetch the authenticated user
        private async Task<User?> FetchUserAndValidateAsync()
        {
            var username = User.Identity?.Name;
            if (string.IsNullOrEmpty(username)) return null;

            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
        }

        // Dashboard landing page
        public IActionResult Index()
        {
            return View();
        }

        // View Claims with Pagination
        public async Task<IActionResult> ViewClaims(int page = 1, int pageSize = 10)
        {
            var user = await FetchUserAndValidateAsync();
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not authenticated.";
                return RedirectToAction("Index", "Home");
            }

            var claims = await _context.Claims
                                       .Where(c => c.UserID == user.UserID)
                                       .Include(c => c.Documents)
                                       .Skip((page - 1) * pageSize)
                                       .Take(pageSize)
                                       .ToListAsync();

            return View(claims);
        }

        // User Feedback
        public async Task<IActionResult> UserFeedback()
        {
            var user = await FetchUserAndValidateAsync();
            if (user == null) return RedirectToAction("Index", "Home");

            var feedbacks = await _context.Feedbacks
                                          .Where(f => f.UserID == user.UserID)
                                          .ToListAsync();

            return View(feedbacks);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitFeedback(string message, int rating)
        {
            var user = await FetchUserAndValidateAsync();
            if (user == null) return RedirectToAction("Index", "Home");

            if (string.IsNullOrEmpty(message) || rating < 1 || rating > 5)
            {
                TempData["ErrorMessage"] = "Invalid feedback. Please provide a valid message and rating.";
                return RedirectToAction("UserFeedback");
            }

            var feedback = new Feedback
            {
                UserID = user.UserID,
                Message = message,
                Rating = rating,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();

            await _hubContext.Clients.All.SendAsync("FeedbackSubmitted", user.UserName, message);
            TempData["SuccessMessage"] = "Feedback submitted successfully!";
            return RedirectToAction("UserFeedback");
        }

        // Upload Documents
        public async Task<IActionResult> UploadDocuments()
        {
            var user = await FetchUserAndValidateAsync();
            if (user == null) return RedirectToAction("Index", "Home");

            var claims = await _context.Claims
                                       .Where(c => c.UserID == user.UserID)
                                       .Include(c => c.Documents)
                                       .ToListAsync();

            return View(claims);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadDocument(int claimID, IFormFile document)
        {
            if (document == null || document.Length == 0)
            {
                TempData["ErrorMessage"] = "Invalid document. Please upload a valid file.";
                return RedirectToAction("UploadDocuments");
            }

            var allowedExtensions = new[] { ".pdf", ".docx", ".xlsx" };
            var fileExtension = Path.GetExtension(document.FileName).ToLower();
            if (!allowedExtensions.Contains(fileExtension))
            {
                TempData["ErrorMessage"] = "Only PDF, DOCX, and XLSX files are allowed.";
                return RedirectToAction("UploadDocuments");
            }

            var user = await FetchUserAndValidateAsync();
            if (user == null) return RedirectToAction("Index", "Home");

            var claim = await _context.Claims.FindAsync(claimID);
            if (claim == null) return NotFound();

            // Sanitize file name
            var sanitizedFileName = Path.GetFileNameWithoutExtension(document.FileName).Replace(" ", "_");
            sanitizedFileName = Regex.Replace(sanitizedFileName, @"[^a-zA-Z0-9_-]", "");
            var uniqueFileName = $"{Guid.NewGuid()}_{sanitizedFileName}{fileExtension}";
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await document.CopyToAsync(fileStream);
            }

            var newDocument = new Document
            {
                ClaimID = claimID,
                DocumentPath = "/uploads/" + uniqueFileName,
                FileName = document.FileName,
                UploadDate = DateTime.Now
            };

            _context.Documents.Add(newDocument);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Document uploaded successfully.";
            await _hubContext.Clients.All.SendAsync("DocumentUploaded", user.UserName, newDocument.FileName);

            return RedirectToAction("ViewClaims");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteDocument(int id)
        {
            var document = await _context.Documents.FindAsync(id);
            if (document == null) return NotFound();

            var filePath = Path.Combine("wwwroot/uploads", document.FileName);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            _context.Documents.Remove(document);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Document deleted successfully.";
            return RedirectToAction("TrackStatus");
        }
        // Get claim details to track status
        public async Task<IActionResult> ClaimDetails(int claimID)
        {
            var claim = await _context.Claims
                                      .Include(c => c.Documents) // Include documents in the query
                                      .FirstOrDefaultAsync(c => c.ClaimID == claimID);
            if (claim == null)
            {
                return NotFound();
            }

            return View(claim);
        }
        // Track Status
        public async Task<IActionResult> TrackStatus()
        {
            var user = await FetchUserAndValidateAsync();
            if (user == null) return RedirectToAction("Index", "Home");

            var claims = await _context.Claims
                                       .Where(c => c.UserID == user.UserID)
                                       .Include(c => c.Documents)
                                       .ToListAsync();

            return View(claims);
        }

        public async Task<IActionResult> UpdateClaimStatus(int claimID, string status)
        {
            var claim = await _context.Claims.FindAsync(claimID);
            if (claim == null) return NotFound();

            claim.Status = status;
            claim.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            await _hubContext.Clients.All.SendAsync("ClaimStatusUpdated", claimID, status);

            TempData["SuccessMessage"] = "Claim status updated.";
            return RedirectToAction("TrackStatus");
        }
        public IActionResult SubmitClaim()
        {
            return View();
        }
        // Submit Claim
        [HttpPost]
        public async Task<IActionResult> SubmitClaim(decimal hoursWorked, decimal hourlyRate, string description)
        {
            var user = await FetchUserAndValidateAsync();
            if (user == null) return RedirectToAction("Index", "Home");

            var amount = hoursWorked * hourlyRate;

            var newClaim = new Models.Claim
            {
                UserID = user.UserID,
                Amount = amount,
                hourlyRate = hourlyRate,
                Hours = hoursWorked,
                Description = description,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Status = "Pending"
            };

            _context.Claims.Add(newClaim);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Claim submitted successfully!";
            return RedirectToAction("ViewClaims");
        }

        public IActionResult Settings()
        {
            return View();
        }
    }

    public class ClaimsHub : Hub { }
}

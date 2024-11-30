using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ST10357066_PROG6212_CMCS_Part1.Data;
using ST10357066_PROG6212_CMCS_Part1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ST10357066_PROG6212_CMCS_Part1.Controllers
{
    public class AcademicManagerController : Controller
    {
        private readonly CMCSDbContext _context;
        private readonly ILogger<AcademicManagerController> _logger;
        private readonly IClaimService _claimService;

        public AcademicManagerController(CMCSDbContext context, ILogger<AcademicManagerController> logger, IClaimService claimService)
        {
            _context = context;
            _logger = logger;
            _claimService = claimService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ClaimsManagement(int page = 1, int pageSize = 10)
        {
            if (page <= 0 || pageSize <= 0)
            {
                return BadRequest("Invalid pagination parameters.");
            }

            var claims = await _context.Claims
                .Include(c => c.User)
                .OrderBy(c => c.ClaimID)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            if (!claims.Any())
            {
                ModelState.AddModelError("", "No claims found.");
                return View(new List<Claim>());
            }

            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            return View(claims);
        }

        public async Task<IActionResult> VerifyClaims()
        {
            await _claimService.VerifyClaimsAsync();
            TempData["Message"] = "Claims verified successfully.";
            return RedirectToAction("ClaimsManagement");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateClaimStatus(int claimID, string actionType)
        {
            try
            {
                var claim = await _context.Claims.FirstOrDefaultAsync(c => c.ClaimID == claimID);
                if (claim == null)
                {
                    return NotFound("Claim not found.");
                }

                // Determine the status based on the actionType
                claim.Status = actionType == "Approve" ? "Approved" : "Rejected";
                claim.UpdatedAt = DateTime.Now;

                _context.Claims.Update(claim);
                await _context.SaveChangesAsync();

                TempData["Message"] = $"Claim {claimID} has been successfully {claim.Status.ToLower()}!";
                _logger.LogInformation($"Claim {claimID} updated to {claim.Status}.");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error updating claim: {ex.Message}";
                _logger.LogError(ex, "Error updating claim status for ClaimID {ClaimID}", claimID);
            }

            return RedirectToAction("ClaimDetails", new { claimID });
        }


        public async Task<IActionResult> UpdateAllClaims(string actionType)
        {
            try
            {
                var claims = await _context.Claims.ToListAsync();
                foreach (var claim in claims)
                {
                    claim.Status = actionType == "Approve" ? "Approved" : "Rejected";
                }
                await _context.SaveChangesAsync();
                TempData["Message"] = $"{actionType} operation completed successfully.";
                _logger.LogInformation("{ActionType} operation completed successfully.", actionType);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error during {actionType} operation: {ex.Message}";
                _logger.LogError(ex, "Error during {ActionType} operation", actionType);
            }
            return RedirectToAction("ClaimsManagement");
        }
        // View Claim
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

        [HttpPost]
        public async Task<IActionResult> ApproveClaim(int claimID)
        {
            var claim = await _context.Claims.FirstOrDefaultAsync(c => c.ClaimID == claimID);
            if (claim == null) return NotFound();

            if (claim.Status != "Pending")
            {
                TempData["Error"] = "Claim is not eligible for approval.";
                return RedirectToAction("ClaimsManagement");
            }

            claim.Status = "Approved";
            _context.Claims.Update(claim);
            await _context.SaveChangesAsync();

            TempData["Message"] = $"Claim {claimID} approved.";
            return RedirectToAction("ClaimsManagement");
        }

        [HttpPost]
        public async Task<IActionResult> RejectClaim(int claimID)
        {
            var claim = await _context.Claims.FirstOrDefaultAsync(c => c.ClaimID == claimID);
            if (claim == null) return NotFound();

            claim.Status = "Rejected";
            _context.Claims.Update(claim);
            await _context.SaveChangesAsync();

            TempData["Message"] = $"Claim {claimID} rejected.";
            return RedirectToAction("ClaimsManagement");
        }
        public IActionResult LecturerDirectory()
        {
            var lecturers = _context.Users.ToList();
            return View(lecturers);
        }

        public IActionResult ReportsAndAnalytics()
        {
            return View();
        }

        public IActionResult Settings()
        {
            return View();
        }

        public IActionResult Profile()
        {
            return View();
        }
    }
}

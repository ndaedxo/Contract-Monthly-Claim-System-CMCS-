using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ST10357066_PROG6212_CMCS_Part1.Data;
using ST10357066_PROG6212_CMCS_Part1.Models;
using System.Diagnostics;

namespace ST10357066_PROG6212_CMCS_Part1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly CMCSDbContext _context;

        public HomeController(ILogger<HomeController> logger, CMCSDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
        // HR View
        public async Task<IActionResult> HR()
        {
            var approvedClaims = await _context.Claims
                .Include(c => c.User)
                .Where(c => c.Status == "Approved")
                .ToListAsync();

            var lecturers = await _context.Users
                .Where(u => u.Role == "Lecturer")
                .ToListAsync();

            ViewBag.ApprovedClaims = approvedClaims;
            return View(lecturers);
        }

        // Generate Invoice for a Claim
        public async Task<IActionResult> GenerateInvoice(int claimId)
        {
            var claim = await _context.Claims
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.ClaimID == claimId && c.Status == "Approved");

            if (claim == null) return NotFound();

            // Generate invoice details
            var invoiceDetails = new
            {
                ClaimID = claim.ClaimID,
                Lecturer = claim.User.UserName,
                Amount = claim.Amount,
                Date = claim.CreatedAt.ToString("MM/dd/yyyy")
            };

            var htmlContent = $@"
            <h1>Invoice for Claim ID: {invoiceDetails.ClaimID}</h1>
            <p><strong>Lecturer:</strong> {invoiceDetails.Lecturer}</p>
            <p><strong>Amount:</strong> {invoiceDetails.Amount:C}</p>
            <p><strong>Date:</strong> {invoiceDetails.Date}</p>";

            return Content(htmlContent, "text/html");
        }

        // Update Lecturer Data
        [HttpPost]
        public async Task<IActionResult> UpdateLecturer(int lecturerId, string name, string email, string phone)
        {
            var lecturer = await _context.Users.FindAsync(lecturerId);

            if (lecturer == null) return NotFound("Lecturer not found.");

            lecturer.Name = name;
            lecturer.Email = email;
            lecturer.Phone = phone;

            _context.Users.Update(lecturer);
            await _context.SaveChangesAsync();

            TempData["Message"] = $"Lecturer {name}'s data updated successfully.";
            return RedirectToAction("HR");
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

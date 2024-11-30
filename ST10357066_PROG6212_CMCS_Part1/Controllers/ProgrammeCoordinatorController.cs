using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ST10357066_PROG6212_CMCS_Part1.Data;
using ST10357066_PROG6212_CMCS_Part1.Models;

namespace ST10357066_PROG6212_CMCS_Part1.Controllers
{
    public class ProgrammeCoordinatorController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly CMCSDbContext _context;
        private readonly ILogger<AccountController> _logger; // Add this

        public ProgrammeCoordinatorController(SignInManager<User> signInManager, UserManager<User> userManager, CMCSDbContext context, ILogger<AccountController> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
            _logger = logger; // Initialize logger
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ClaimsManagement()
        {
            return View();
        }

        public IActionResult LecturerDirectory()
        {
            return View();
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
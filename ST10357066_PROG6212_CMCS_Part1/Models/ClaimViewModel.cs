using System.ComponentModel.DataAnnotations;

namespace ST10357066_PROG6212_CMCS_Part1.Models
{
    public class ClaimViewModel
    {
        [Required]
        public int HoursWorked { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Hourly rate must be positive.")]
        public decimal HourlyRate { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; }

        public List<IFormFile> Documents { get; set; }
    }
}

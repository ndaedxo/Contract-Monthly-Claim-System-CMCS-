using System.ComponentModel.DataAnnotations;

namespace ST10357066_PROG6212_CMCS_Part1.Models
{
    public class LoginViewModel
    {

        [Required]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }


        // Add the RememberMe property
        public bool RememberMe { get; set; }
    }
}

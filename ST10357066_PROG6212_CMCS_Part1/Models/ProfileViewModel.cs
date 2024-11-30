using System.ComponentModel.DataAnnotations;

namespace ST10357066_PROG6212_CMCS_Part1.Models
{
    public class ProfileViewModel
    {
        public string? UserName { get; set; } = string.Empty;
        public string? FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        public string? Password { get; set; } = string.Empty;// Optional password change
    }

}

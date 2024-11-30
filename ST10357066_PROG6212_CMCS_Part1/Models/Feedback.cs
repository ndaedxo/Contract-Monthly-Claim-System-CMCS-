using System.ComponentModel.DataAnnotations;

namespace ST10357066_PROG6212_CMCS_Part1.Models
{
    public class Feedback
    {
        [Key]
        public int FeedbackID { get; set; } // Primary key

        public int UserID { get; set; } // Foreign key to User

        [Required(ErrorMessage = "Message is required")]
        [StringLength(1000, ErrorMessage = "Message can't be longer than 1000 characters.")]
        public string Message { get; set; } = string.Empty;

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; } // Rating from 1 to 5

        public DateTime CreatedAt { get; set; } = DateTime.Now; // Automatically set to current date/time
        public DateTime UpdatedAt { get; set; } = DateTime.Now; // Automatically updated

        // Navigation property for User
        public User? User { get; set; }

        public Feedback() { } // Default constructor for EF Core

        public Feedback(int userId, string message, int rating)
        {
            UserID = userId;
            Message = message;
            Rating = rating;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
    }
}

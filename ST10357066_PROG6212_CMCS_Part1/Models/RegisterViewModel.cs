﻿using System.ComponentModel.DataAnnotations;

namespace ST10357066_PROG6212_CMCS_Part1.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }
    }

}

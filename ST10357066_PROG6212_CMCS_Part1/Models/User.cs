using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System;
using System.Collections.Generic;

namespace ST10357066_PROG6212_CMCS_Part1.Models
{
    public class User 
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; } = string.Empty; // You will need to handle hashing manually
        public string Email { get; set; } 
        public string? FirstName { get; set; } 
        public string? LastName { get; set; }
        public string? Role { get; set; }
        public string? Phone { get; set; }
        public DateTime CreatedAt { get; set; } 
        public DateTime UpdatedAt { get; set; } 

        // Navigation property for the Role
        public ICollection<UserRole> UserRoles { get; set; } // UserRoles to handle many-to-many relationship

        public ICollection<Approval> Approvals { get; set; } 
        public ICollection<Claim> Claims { get; set; }  // Initialize as an empty list

        public User() {

            UserName =  string.Empty;
            Email =  string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            PasswordHash = string.Empty;
            Role = string.Empty;
            Name = string.Empty;
            Phone = string.Empty;

            UserRoles = new List<UserRole>();
            Approvals = new HashSet<Approval>();
            Claims = new List<Claim>();
        }

        public User(string username, string email, string firstName, string lastName)
        {
            UserName = username;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            Role = string.Empty;
            Name = string.Empty;
            Phone = string.Empty;

            UserRoles  = new List<UserRole>();
            Approvals = new HashSet<Approval>();
            Claims = new List<Claim>();
        }

        public override string ToString()
        {
            return $"UserID: {UserID}, Username: {UserName}, Email: {Email}, Name: {FirstName} {LastName}, CreatedAt: {CreatedAt}";
        }
    }
}

using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace ST10357066_PROG6212_CMCS_Part1.Models
{
    public class Role 
    {
        public int RoleID { get; set; }
        public string Name { get; set; } 
        // Navigation property for the UserRoles
        public ICollection<UserRole> UserRoles { get; set; }  // UserRoles to handle many-to-many relationship

        public Role()  {
            Name = string.Empty;
            UserRoles = new List<UserRole>();
        }

        public Role(string name) { 
            Name = name;
            UserRoles = new List<UserRole>();
        }

        public override string ToString()
        {
            return $"RoleID: {RoleID}, RoleName: {Name}";
        }
    }
}

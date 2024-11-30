namespace ST10357066_PROG6212_CMCS_Part1.Models
{
    public class UserRole
    {
        public int UserRoleID { get; set; }
        public int UserID { get; set; }
        public int RoleID { get; set; }
        public DateTime AssignedAt { get; set; } = DateTime.Now;  // Default to the current date and time

        // Navigation property for the User
        public User User { get; set; }  // Corrected type from string to User

        // Navigation property for the Role
        public Role Role { get; set; }  // Corrected type from string to Role

        public UserRole() { 
        }

        public UserRole(int userID, int roleID)
        {
            UserID = userID;
            RoleID = roleID;
            AssignedAt = DateTime.Now;
        }

        public override string ToString()
        {
            return $"UserRoleID: {UserRoleID}, UserID: {UserID}, RoleID: {RoleID}, AssignedAt: {AssignedAt}";
        }
    }
}

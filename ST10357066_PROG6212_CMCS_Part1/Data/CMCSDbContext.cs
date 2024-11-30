using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Reflection.Metadata;
using System.Security.Claims;
using ST10357066_PROG6212_CMCS_Part1.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace ST10357066_PROG6212_CMCS_Part1.Data
{
    public class CMCSDbContext : DbContext
    {
        public CMCSDbContext(DbContextOptions<CMCSDbContext> options) : base(options)
        {
        }

        // Define DbSets for the entities
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Models.Claim> Claims { get; set; }
        public DbSet<Models.Document> Documents { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Approval> Approvals { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure unique constraints
            modelBuilder.Entity<Role>()
                .HasIndex(r => r.Name)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.UserName)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Setting>()
                .HasIndex(s => s.SettingName)
                .IsUnique();

            modelBuilder.Entity<Approval>()
                .HasIndex(a => a.Status)
                .IsUnique(false);

            // Configure relationships between User and Claims
            modelBuilder.Entity<Models.Claim>()
                .HasOne(c => c.User)
                .WithMany(u => u.Claims)
                .HasForeignKey(c => c.UserID);

            // Configure relationships between Claim and Documents
            modelBuilder.Entity<Models.Document>()
                .HasOne(d => d.Claim)
                .WithMany(c => c.Documents)
                .HasForeignKey(d => d.ClaimID);

            // Configure relationships for UserRole
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleID)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Approval relationships
            modelBuilder.Entity<Approval>()
                .HasOne(a => a.ApprovedByUser)
                .WithMany(u => u.Approvals)
                .HasForeignKey(a => a.UserID)
                .IsRequired(false);  // Optional relationship

            // Specify column types and precision for decimal properties
            modelBuilder.Entity<Models.Claim>()
                .Property(c => c.Amount)  // Ensure you have Amount property in your Claim model
                .HasColumnType("decimal(18,2)"); // Adjust precision and scale as needed

            // Explicitly map entities to their respective tables
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Models.Claim>().ToTable("Claim");
            modelBuilder.Entity<Models.Document>().ToTable("Document");
            modelBuilder.Entity<Models.Approval>().ToTable("Approval");
            modelBuilder.Entity<Models.Role>().ToTable("Role");
        }
    }
}
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using ST10357066_PROG6212_CMCS_Part1.Controllers;
using ST10357066_PROG6212_CMCS_Part1.Data;
using ST10357066_PROG6212_CMCS_Part1.Models;

namespace ST10357066_PROG6212_CMCS_Part1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure logging
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.Logging.AddDebug();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Register the IClaimService with its implementation
            builder.Services.AddScoped<IClaimService, ClaimService>();


            // Configure Entity Framework
            builder.Services.AddDbContext<CMCSDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Cookie-based authentication configuration
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.LogoutPath = "/Account/Logout";
                    options.AccessDeniedPath = "/Account/AccessDenied";
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Set session timeout (example: 30 mins)
                });

            // Add SignalR to the service container
            builder.Services.AddSignalR();

            var app = builder.Build();

            // Map SignalR hubs
            app.MapHub<ClaimsHub>("/claimsHub");

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles(); // Ensure static files (e.g., PDFs in wwwroot) are served

            app.UseRouting();

            // Enable Authentication and Authorization
            app.UseAuthentication();  // Ensure authentication is applied
            app.UseAuthorization();

            // Configure default route for your MVC controllers
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");


            // Ensure files in wwwroot/Docs can be accessed
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Docs")),
                RequestPath = "/Docs"
            });

            app.Run();
        }
    }
}

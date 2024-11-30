using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using ST10357066_PROG6212_CMCS_Part1.Controllers;
using ST10357066_PROG6212_CMCS_Part1.Data;
using ST10357066_PROG6212_CMCS_Part1.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace TestProject
{
    public class LecturerDashboardControllerTests
    {
        private readonly Mock<ILogger<LecturerDashboardController>> _loggerMock;
        private readonly CMCSDbContext _context;
        private readonly LecturerDashboardController _controller;

        public LecturerDashboardControllerTests()
        {
            var options = new DbContextOptionsBuilder<CMCSDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            _context = new CMCSDbContext(options);
            _loggerMock = new Mock<ILogger<LecturerDashboardController>>();
            _controller = new LecturerDashboardController(_context, _loggerMock.Object);
        }

        private void SetupUser(string username)
        {
            var userClaims = new List<System.Security.Claims.Claim>
            {
                new System.Security.Claims.Claim(ClaimTypes.Name, username)
            };
            var claimsIdentity = new ClaimsIdentity(userClaims, "TestAuth");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            _controller.ControllerContext.HttpContext = new DefaultHttpContext { User = claimsPrincipal };
        }

        [Fact]
        public void Index_ReturnsViewResult()
        {
            var result = _controller.Index();

            Xunit.Assert.IsType<ViewResult>(result); // Fully qualify Xunit.Assert
        }

        [Fact]
        public async Task ViewClaims_UserNotAuthenticated_ReturnsViewWithError()
        {
            var result = await _controller.ViewClaims();

            var viewResult = Xunit.Assert.IsType<ViewResult>(result); // Fully qualify Xunit.Assert
            Xunit.Assert.False(viewResult.ViewData.ModelState.IsValid); // Fully qualify Xunit.Assert
            Xunit.Assert.Contains(viewResult.ViewData.ModelState, e => e.Key == ""); // Fully qualify Xunit.Assert
        }

        [Fact]
        public async Task ViewClaims_UserExists_NoClaims_ReturnsEmptyList()
        {
            SetupUser("testuser");
            _context.Users.Add(new User { UserName = "testuser", UserID = 1 });
            await _context.SaveChangesAsync();

            var result = await _controller.ViewClaims();

            var viewResult = Xunit.Assert.IsType<ViewResult>(result); // Fully qualify Xunit.Assert
            var claims = Xunit.Assert.IsAssignableFrom<List<ST10357066_PROG6212_CMCS_Part1.Models.Claim>>(viewResult.Model); // Use fully qualified name
            Xunit.Assert.Empty(claims); // Fully qualify Xunit.Assert
        }

        [Fact]
        public async Task UserFeedback_UserNotAuthenticated_ReturnsViewWithError()
        {
            var result = await _controller.UserFeedback();

            var viewResult = Xunit.Assert.IsType<ViewResult>(result); // Fully qualify Xunit.Assert
            Xunit.Assert.False(viewResult.ViewData.ModelState.IsValid); // Fully qualify Xunit.Assert
        }

        [Fact]
        public async Task UserFeedback_ReturnsUserFeedback()
        {
            SetupUser("testuser");
            var user = new User { UserName = "testuser", UserID = 1 };
            _context.Users.Add(user);
            _context.Feedbacks.Add(new Feedback { UserID = 1, Message = "Test feedback", Rating = 5 });
            await _context.SaveChangesAsync();

            var result = await _controller.UserFeedback();

            var viewResult = Xunit.Assert.IsType<ViewResult>(result); // Fully qualify Xunit.Assert
            var feedbacks = Xunit.Assert.IsAssignableFrom<List<Feedback>>(viewResult.Model); // Fully qualify Xunit.Assert
            Xunit.Assert.Single(feedbacks); // Fully qualify Xunit.Assert
        }

        [Fact]
        public async Task SubmitFeedback_UserNotAuthenticated_ReturnsViewWithError()
        {
            var result = await _controller.SubmitFeedback("Test feedback", 5);

            var viewResult = Xunit.Assert.IsType<ViewResult>(result); // Fully qualify Xunit.Assert
            Xunit.Assert.False(viewResult.ViewData.ModelState.IsValid); // Fully qualify Xunit.Assert
        }

        [Fact]
        public async Task SubmitFeedback_ValidFeedback_SuccessfulSubmission()
        {
            SetupUser("testuser");
            var user = new User { UserName = "testuser", UserID = 1 };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var result = await _controller.SubmitFeedback("Great service!", 5);

            var redirectResult = Xunit.Assert.IsType<RedirectToActionResult>(result); // Fully qualify Xunit.Assert
            Xunit.Assert.Equal("UserFeedback", redirectResult.ActionName); // Fully qualify Xunit.Assert
        }

        [Fact]
        public async Task UploadDocuments_UserNotAuthenticated_ReturnsViewWithError()
        {
            var result = await _controller.UploadDocuments();

            var viewResult = Xunit.Assert.IsType<ViewResult>(result); // Fully qualify Xunit.Assert
            Xunit.Assert.False(viewResult.ViewData.ModelState.IsValid); // Fully qualify Xunit.Assert
        }

        [Fact]
        public async Task UploadDocuments_NoClaimsFound_ReturnsEmptyList()
        {
            SetupUser("testuser");
            var user = new User { UserName = "testuser", UserID = 1 };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var result = await _controller.UploadDocuments();

            var viewResult = Xunit.Assert.IsType<ViewResult>(result); // Fully qualify Xunit.Assert
            var claims = Xunit.Assert.IsAssignableFrom<List<ST10357066_PROG6212_CMCS_Part1.Models.Claim>>(viewResult.Model); // Use fully qualified name
            Xunit.Assert.Empty(claims); // Fully qualify Xunit.Assert
        }

        // Additional tests for other methods (TrackStatus, ClaimDetails, etc.) can be added here...
    }
}

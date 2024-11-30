using Microsoft.EntityFrameworkCore;
using ST10357066_PROG6212_CMCS_Part1.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ST10357066_PROG6212_CMCS_Part1.Models
{
    public interface IClaimService
    {
        Task VerifyClaimsAsync();
    }

    public class ClaimService : IClaimService
    {
        private readonly CMCSDbContext _context;

        public ClaimService(CMCSDbContext context)
        {
            _context = context;
        }

        public async Task VerifyClaimsAsync()
        {
            var claims = await _context.Claims.Include(c => c.User).ToListAsync();
            foreach (var claim in claims)
            {
                // Validate Hours Worked
                if (claim.Hours > 40)
                {
                    claim.Status = "Rejected";
                    claim.ValidationError = "Exceeded allowable working hours.";
                }
                // Validate Hourly Rate
                else if (claim.hourlyRate < 100 || claim.hourlyRate > 500)
                {
                    claim.Status = "Rejected";
                    claim.ValidationError = "Hourly rate is out of range.";
                }
                // Validate Submission Date
                else if ((DateTime.Now - claim.CreatedAt).TotalDays > 30)
                {
                    claim.Status = "Rejected";
                    claim.ValidationError = "Claim submitted beyond the allowable timeframe.";
                }
                else
                {
                    claim.Status = "Pending";
                    claim.ValidationError = null;
                }

                _context.Claims.Update(claim);
            }

            await _context.SaveChangesAsync();
        }
    }

}

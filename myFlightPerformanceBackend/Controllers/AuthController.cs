using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using myFlightPerformance.Models;

namespace myFlightPerformance.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher<Instructor> _passwordHasher;

        public AuthController(ApplicationDbContext context, IPasswordHasher<Instructor> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [EnableRateLimiting("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var instructor = await _context.Instructors
                .SingleOrDefaultAsync(i => i.Username == request.Username.Trim().ToLower());

            if (instructor == null)
            {
                return Unauthorized(new { message = "Invalid username or password" });
            }

            var result = _passwordHasher.VerifyHashedPassword(instructor, instructor.PasswordHash, request.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                return Unauthorized(new { message = "Invalid username or password" });
            }

            // Re-hash when the stored hash uses older settings.
            if (result == PasswordVerificationResult.SuccessRehashNeeded)
            {
                instructor.PasswordHash = _passwordHasher.HashPassword(instructor, request.Password);
                await _context.SaveChangesAsync();
            }

            var identity = new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.NameIdentifier, instructor.Id.ToString()),
                    new Claim(ClaimTypes.Name, instructor.Username),
                ],
                CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            return Ok(new { username = instructor.Username });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return NoContent();
        }

        [HttpGet("me")]
        public IActionResult Me()
        {
            return Ok(new { username = User.Identity?.Name });
        }

        // Signed-in instructors can add other instructors.
        [HttpPost("instructors")]
        public async Task<IActionResult> CreateInstructor(CreateInstructorRequest request)
        {
            var username = request.Username.Trim().ToLower();
            if (await _context.Instructors.AnyAsync(i => i.Username == username))
            {
                return Conflict(new { message = "That username is already taken" });
            }

            var instructor = new Instructor
            {
                Username = username,
                PasswordHash = "",
                CreatedAt = DateTime.UtcNow,
            };
            instructor.PasswordHash = _passwordHasher.HashPassword(instructor, request.Password);

            _context.Instructors.Add(instructor);
            await _context.SaveChangesAsync();

            return Ok(new { username = instructor.Username });
        }
    }
}

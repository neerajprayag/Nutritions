using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Nutritions.Model;
using Nutritions.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;



namespace Nutritions.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly NutritionDbContext _context;
        private readonly IConfiguration _configuration;

        public UserController(NutritionDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // Register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
                return BadRequest("Email is already registered.");

            var user = new User
            {
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Role = "User",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return Ok("Registration successful.");
        }

        // Login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                // Log input
                Console.WriteLine($"Login attempt: Email = {request.Email}");

                // Validate input
                if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
                    return BadRequest("Email and Password are required.");

                // Fetch user from the database
                Console.WriteLine($"Executing query: SELECT * FROM Users WHERE Email = '{request.Email}'");
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

                if (user == null)
                {
                    Console.WriteLine($"No user found with email: {request.Email}");
                    return Unauthorized("Invalid email or password.");
                }

                // Log user data
                Console.WriteLine($"User found: {user.Email}, Role: {user.Role}, UserId: {user.UserId}");

                // Check if the PasswordHash is valid
                if (string.IsNullOrEmpty(user.PasswordHash))
                {
                    Console.WriteLine($"No password hash found for user: {user.Email}");
                    return Unauthorized("User does not have a valid password.");
                }

                // Verify password
                if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                {
                    Console.WriteLine($"Invalid password for user: {user.Email}");
                    return Unauthorized("Invalid email or password.");
                }

                // Generate JWT token
                var token = GenerateJwtToken(user);

                // Log successful login
                Console.WriteLine($"User {user.Email} logged in successfully.");
                return Ok(new { Token = token, User = new { user.UserId, user.Email, user.FirstName, user.LastName, user.Role } });
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
                return StatusCode(500, "A database error occurred.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during login: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                return StatusCode(500, "Internal server error.");
            }
        }




        // Get Profile
        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        // Update Profile
        [HttpPut("profile")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
                return NotFound();

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.ProfilePicture = request.ProfilePicture;
            user.UpdatedAt = DateTime.Now;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }

    
}

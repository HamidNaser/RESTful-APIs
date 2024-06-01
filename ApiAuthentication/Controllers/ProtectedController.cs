using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration; // Add this namespace
using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Authenication
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProtectedController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ProtectedController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Load secret key from configuration
        private string GetSecretKey()
        {
            return _configuration["AppSettings:JwtSecretKey"]; // Adjust key as per your appsettings.json structure
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult GetProtectedData()
        {
            var secretKey = GetSecretKey();

            // Extract user information from JWT token
            var userClaims = HttpContext.User.Claims;
            var username = userClaims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var role = userClaims?.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            // Perform role-based authorization checks
            if (role != "admin")
            {
                return Forbid(); // Return 403 Forbidden if the user does not have the required role
            }

            // Return protected data
            return Ok($"Hello, {username}! This is protected data for users with 'admin' role.");
        }
    }
}

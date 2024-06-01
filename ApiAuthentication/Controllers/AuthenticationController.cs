using Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Authenication
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;


        public AuthController(IConfiguration configuration)
        {
            _secretKey = configuration["AppSettings:JwtSecretKey"];
            _issuer = configuration["AppSettings:Issuer"];
            _audience = configuration["AppSettings:Audience"];

        }

        private readonly List<User> _users = new List<User>
        {
            new User { Id = 1, Username = "admin", Password = "admin123", Role = "admin" },
            new User { Id = 2, Username = "user", Password = "user123", Role = "user" }
        };


        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public object Login([FromBody] UserLogin model)
        {
            var user = AuthenticateUser(model.Username, model.Password);

            if (user == null)
            {
                return Unauthorized();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };


            var token = JwtHelper.GetJwtToken(
                user.Username,
                _secretKey,
                _issuer,
                _audience,
                TimeSpan.FromMinutes(1),
                claims.ToArray());

            return new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expires = token.ValidTo
            };
        }

        private User AuthenticateUser(string username, string password)
        {
            return _users.Find(u => u.Username == username && u.Password == password);
        }
    }

    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }

    public class UserLogin
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}

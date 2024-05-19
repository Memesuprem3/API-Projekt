using API_Projekt.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V5.Pages.Account.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Models;
using Projekt_Avancerad_.Net_Bokning.Data;
using Projekt_Avancerad_.Net_Bokning.Services.Interface;
using Projekt_Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Projekt_Avancerad_.Net_Bokning.Services
{
    public class AuthenticationRepo : IAuthentication
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthenticationRepo(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<IdentityResult> RegisterAsync(User userModel)
        {
            var userExists = await _context.Users.AnyAsync(u => u.Username == userModel.Username);
            if (userExists)
                return IdentityResult.Failed(new IdentityError { Description = "User already exists!" });

            var user = new User
            {
                Username = userModel.Username,
                Email = userModel.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userModel.PasswordHash),
                Role = userModel.Role,
                IsActive = true
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return IdentityResult.Success;
        }

        public async Task<string> LoginAsync(User loginModel)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == loginModel.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginModel.PasswordHash, user.PasswordHash))
                return null;

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(authClaims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
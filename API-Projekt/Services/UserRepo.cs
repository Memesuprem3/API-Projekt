using API_Projekt.Services.Interface;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Models;
using Projekt_Avancerad_.Net_Bokning.Data;
using Projekt_Avancerad_.Net_Bokning.Services.Interface;
using Projekt_Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

public class UserRepo : IUser
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public UserRepo(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<User> GetUserByUsernameAsync(string username)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task AddUserAsync(User user)
    {
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ValidateUserCredentialsAsync(string username, string password)
    {
        var user = await GetUserByUsernameAsync(username);
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            return false;
        }
        return true;
    }

    public async Task<string> GenerateJwtTokenAsync(User user)
    {
        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            expires: DateTime.Now.AddHours(3),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
using Microsoft.AspNetCore.Mvc;
using Projekt_Models;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private UserRepo _userRepo;

    public AuthController(UserRepo userRepo)
    {
        _userRepo = userRepo;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] User userModel)
    {
        var userExists = await _userRepo.GetUserByUsernameAsync(userModel.Username);
        if (userExists != null)
        {
            return StatusCode(500, "User already exists!");
        }

        var user = new User
        {
            Username = userModel.Username,
            Email = userModel.Email,
            PasswordHash = userModel.PasswordHash,
            Role = userModel.Role,
            IsActive = true
        };

        await _userRepo.AddUserAsync(user);
        return Ok("User created successfully!");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] User loginModel)
    {
        if (!await _userRepo.ValidateUserCredentialsAsync(loginModel.Username, loginModel.PasswordHash))
        {
            return Unauthorized();
        }

        var user = await _userRepo.GetUserByUsernameAsync(loginModel.Username);
        var token = await _userRepo.GenerateJwtTokenAsync(user);
        return Ok(new { Token = token });
    }
}
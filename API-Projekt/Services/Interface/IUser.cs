using Models;

namespace API_Projekt.Services.Interface
{
    public interface IUser
    {
        Task<User> GetUserByUsernameAsync(string username);
        Task AddUserAsync(User user);
        Task<bool> ValidateUserCredentialsAsync(string username, string password);
        Task<string> GenerateJwtTokenAsync(User user);
    }
}

using Microsoft.AspNetCore.Identity;
using Models;

namespace API_Projekt.Services.Interface
{
    public interface IAuthentication
    {
        Task<IdentityResult> RegisterAsync(User userModel);
        Task<string> LoginAsync(User loginModel);
    }
}

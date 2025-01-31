using PairProgress.Backend.Models;

namespace PairProgress.Backend.Services.Interfaces;

public interface IAuthService
{
    Task<string> Login(LoginModel loginModel);  
    Task<bool> Register(RegisterUserModel registerUserModel);
}
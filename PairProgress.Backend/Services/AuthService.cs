using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using PairProgress.Backend.Models;
using PairProgress.Backend.Services.Interfaces;

namespace PairProgress.Backend.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IConfiguration _configuration;

    public AuthService(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }
    
    public async Task<string> Login(LoginModel loginModel)
    {
        var user = await _userManager.FindByNameAsync(loginModel.UserName);
        
        if (user == null)
        {
            return null;
        }
        
        var result = await _signInManager.CheckPasswordSignInAsync(user, loginModel.Password, false);
        
        if (result.Succeeded)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        
        return null;
    }

    public async Task<bool> Register(RegisterUserModel registerUserModel)
    {
        var user = new User()
        {
            UserName = registerUserModel.UserName,
            Email = registerUserModel.Email
        };
        
        var result = await _userManager.CreateAsync(user, registerUserModel.Password);

        if (result.Succeeded)
        {
            return true;
        }
        
        return false;
    }
}
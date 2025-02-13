using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PairProgress.Backend.Data;
using PairProgress.Backend.Models;
using PairProgress.Backend.Services.Interfaces;

namespace PairProgress.Backend.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _dbContext;

        public AuthService(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration, AppDbContext dbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _dbContext = dbContext;
        }

        public async Task<string> Login(LoginModel loginModel)
        {
            var user = await _userManager.FindByNameAsync(loginModel.UserName);

            if (user == null)
            {
                throw new PersonalizedException("User not found.");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginModel.Password, false);

            if (!result.Succeeded)
            {
                throw new PersonalizedException("Invalid username or password.");
            }

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration["Jwt:SigningKey"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, user.UserCode)
                    }),
                    Expires = DateTime.UtcNow.AddHours(2),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                throw new PersonalizedException("Error generating token.", ex);
            }
        }

        public async Task<bool> Register(RegisterUserModel registerUserModel)
        {
            // Check if email already exists
            if (await _userManager.FindByEmailAsync(registerUserModel.Email) != null)
            {
                throw new PersonalizedException("Email already exists.");
            }

            // Check if username already exists
            if (await _userManager.FindByNameAsync(registerUserModel.UserName) != null)
            {
                throw new PersonalizedException("Username already exists.");
            }

            var existingCodes = await _dbContext.Users.Select(u => u.UserCode).ToListAsync();
            var userCode = GenerateUniqueCode(existingCodes);

            var user = new User()
            {
                UserName = registerUserModel.UserName,
                Email = registerUserModel.Email,
                UserCode = userCode
            };

            var result = await _userManager.CreateAsync(user, registerUserModel.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(" - ", result.Errors.Select(e => e.Description));
                throw new PersonalizedException($"Error registering user: {errors}");
            }

            return true;
        }

        private string GenerateUniqueCode(List<string> existingCodes)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            string newCode;

            do
            {
                newCode = new string(Enumerable.Repeat(chars, 5)
                    .Select(s => s[random.Next(s.Length)]).ToArray());
            } while (existingCodes.Contains(newCode));

            return newCode;
        }
    }
}
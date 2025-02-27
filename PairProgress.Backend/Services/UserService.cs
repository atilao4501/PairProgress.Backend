using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PairProgress.Backend.Data;
using PairProgress.Backend.Models;
using PairProgress.Backend.Services.Interfaces;

namespace PairProgress.Backend.Services;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly AppDbContext _context;

    public UserService(UserManager<User> userManager, AppDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task EditUserByCode(UpdateUserInput updateUserInput)
    {
        var userDb = await _userManager.Users.FirstOrDefaultAsync(u => u.UserCode == updateUserInput.UserCode);
        
        if (userDb == null)
        {
            throw new PersonalizedException("User not found with the provided code.");
        }
        
        userDb.UserName = updateUserInput.Username;
        userDb.Email = updateUserInput.Email;

        await _userManager.UpdateAsync(userDb);
        
    }

    public bool CheckIfUsernameExists(string username)
    {
        var userDb = _userManager.Users.FirstOrDefault(u => u.UserName == username);
        if (userDb != null)
        {
            return true;
        }
        else return false;
    }

    public bool CheckIfEmailExists(string email)
    {
        var userDb = _userManager.Users.FirstOrDefault(u => u.Email == email);
        if (userDb != null)
        {
            return true;
        }
        else return false;
    }

    public async Task<GetUserOutput> GetUserByUserCode(string userCode)
    {
        if (userCode == null)
        {
            throw new PersonalizedException("Invalid token.");
        }
        
        var userDb = await _userManager.Users.FirstOrDefaultAsync(u => u.UserCode == userCode);
    
        if (userDb == null)
        {
            throw new PersonalizedException("User not found with the provided code.");
        }
        
        var userReturn = new GetUserOutput
        {
            UserName = userDb.UserName,
            UserCode = userDb.UserCode,
            Email = userDb.Email,
        };
        
        var userDuo = await _context.UserDuos
            .FirstOrDefaultAsync(p => p.User1Code == userCode || p.User2Code == userCode);
    
        if (userDuo != null)
        {
            var pairCode = userDuo.User1Code == userCode ? userDuo.User2Code : userDuo.User1Code;
            
            var userPairDb = await _userManager.Users.FirstOrDefaultAsync(u => u.UserCode == pairCode);
        
            if (userPairDb != null)
            {
                userReturn.PairCode = userPairDb.UserCode;
                userReturn.PairEmail = userPairDb.Email;
                userReturn.PairName = userPairDb.UserName;
            }
        }

        return userReturn;
    }
}
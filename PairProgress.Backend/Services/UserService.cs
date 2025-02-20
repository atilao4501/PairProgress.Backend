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
    
    public async Task<GetUserOutput> GetUserByCode(string userCode)
    {
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

        var dbDuo =
            await _context.UserDuos
                .Include(p => p.User1)
                .Include(p => p.User2)
                .FirstOrDefaultAsync(p => p.User1Code == userCode || p.User2Code == userCode);
        
        
        if (dbDuo != null)
        {
            string pairCode = dbDuo.User1Code;
            
            if (dbDuo.User1Code == userCode)
            {
                pairCode = dbDuo.User2Code;
            }
           var userPairDb = await  _userManager.Users.FirstOrDefaultAsync(u => u.UserCode == pairCode);
           
              if (userPairDb != null)
              {
                  userReturn.PairCode = userPairDb.UserCode;
                  userReturn.PairEmail = userPairDb.Email;
                  userReturn.PairName = userPairDb.UserName;
              }
        }

        return userReturn;

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
}
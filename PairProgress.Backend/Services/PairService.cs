using Microsoft.EntityFrameworkCore;
using PairProgress.Backend.Data;
using PairProgress.Backend.Models;
using PairProgress.Backend.Services.Interfaces;

namespace PairProgress.Backend.Services;
public class PairService : IPairService
{
    private readonly AppDbContext _dbcontext;

    public PairService(AppDbContext dbContext)
    {
        _dbcontext = dbContext;
    }
    
    public async Task<bool> CreatePair(string userCode1, string userCode2)
    {
        if (userCode1 == userCode2)
        {
            throw new PersonalizedException("The user codes cannot be the same.");
        }
        
        if (await _dbcontext.UserDuos.AnyAsync(u => 
                (u.User1Code == userCode1 || u.User2Code == userCode1) || 
                (u.User1Code == userCode2 || u.User2Code == userCode2)))
        {
            throw new PersonalizedException("One of the user is already on a pair.");
        }
        
        var userDuo = new UserDuo
        {
            User1Code = userCode1,
            User2Code = userCode2
        };

        await _dbcontext.UserDuos.AddAsync(userDuo);
        await _dbcontext.SaveChangesAsync();
        
        return true;
    }

    public async Task<bool> DeletePair(string userCode)
    {
        userCode = userCode.ToUpper();
        var pairDb = await  _dbcontext.UserDuos.FirstOrDefaultAsync(u => u.User1Code.ToUpper() == userCode || u.User2Code.ToUpper() == userCode);

        if (pairDb == null)
        {
            throw new PersonalizedException("The user with code: " + userCode + " has no pair.");
        }
        
        _dbcontext.UserDuos.Remove(pairDb);
        await _dbcontext.SaveChangesAsync();

        return true;
    }

    public async Task<UserDuo> GetPairByUserCode(string userCode)
    {
        var pairDb = await _dbcontext.UserDuos.FirstOrDefaultAsync(u => u.User1Code == userCode || u.User2Code == userCode);
        
        if (pairDb == null)
        {
            throw new PersonalizedException("The user with code: " + userCode + " has no pair.");
        }

        return pairDb;

    }
}
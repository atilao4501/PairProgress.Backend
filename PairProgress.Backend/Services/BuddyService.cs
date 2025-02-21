using Microsoft.EntityFrameworkCore;
using PairProgress.Backend.Data;
using PairProgress.Backend.Models;
using PairProgress.Backend.Models.Enum;
using PairProgress.Backend.Services.Interfaces;

namespace PairProgress.Backend.Services;

public class BuddyService :IBuddyService
{
    private readonly AppDbContext _dbContext;

    public BuddyService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<BuddyMoodEnum> GetBuddyMoodForUserAsync(string userCode)
    {
        var userCodesTuple = (userCode, userCode);

        var userDuoDb = _dbContext.UserDuos.FirstOrDefault(ud => ud.User1Code == userCode || ud.User2Code == userCode);

        if (userDuoDb != null)
        {
            userCodesTuple = (userDuoDb.User1Code, userDuoDb.User2Code);
        }
        
        var goals = await _dbContext.Goals
            .Include(g => g.User)
            .Where(g => g.User.UserCode == userCodesTuple.Item1 || g.User.UserCode == userCodesTuple.Item2)
            .ToListAsync();
        
        var contributions = await _dbContext.Contributions
            .Where(c => c.User.UserCode == userCode)
            .ToListAsync();

        if (contributions == null || contributions.Count == 0)
        {
            return BuddyMoodEnum.Angry;
        }

        var contributionsOnThisMonth = contributions.Count(c => c.Date.Month == DateTime.Now.Month && c.Date.Year == DateTime.Now.Year);

        if (contributionsOnThisMonth == 0)
        {
            return BuddyMoodEnum.Angry;
        }
        else if (contributionsOnThisMonth >= (int)Math.Ceiling(goals.Count / 2.0))
        {
            return BuddyMoodEnum.Happy;
        }

        return BuddyMoodEnum.Normal;
    }
}
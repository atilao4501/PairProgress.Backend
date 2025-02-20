using Microsoft.EntityFrameworkCore;
using PairProgress.Backend.Data;
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
        var contributions = await _dbContext.Contributions
            .Where(c => c.User.UserCode == userCode)
            .ToListAsync();

        var contributionsOnThisMonth = contributions
            .Count(c => c.Date.Month == DateTime.Now.Month);
        
        if (contributionsOnThisMonth == 0)
        {
            return BuddyMoodEnum.Angry;
        }
        else if(contributionsOnThisMonth >= (contributions.Count/2))
        {
            return BuddyMoodEnum.Happy;
        }

        return BuddyMoodEnum.Normal;
    }
}
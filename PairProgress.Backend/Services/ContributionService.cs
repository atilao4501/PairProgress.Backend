using Microsoft.EntityFrameworkCore;
using PairProgress.Backend.Data;
using PairProgress.Backend.Models;
using PairProgress.Backend.Services.Interfaces;

namespace PairProgress.Backend.Services;

public class ContributionService : IContributionService
{
    private readonly AppDbContext _dbContext;

    public ContributionService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task AddContributionAsync(CreateContributionInput contributionInput)
    {
        var goalDb = await _dbContext.Goals
            .Include(g => g.User)
            .FirstOrDefaultAsync(g => g.Id == contributionInput.GoalId);
        if (goalDb == null)
        {
            throw new PersonalizedException("Goal not found");
        }
        
        var contribution = new Contribution
        {
            Goal = goalDb,
            Amount = contributionInput.Amount,
            Date = contributionInput.Date,
            User = goalDb.User
        };
        
        await _dbContext.Contributions.AddAsync(contribution);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Contribution>> GetContributionsByGoalAsync(int goalId)
    {
        var goalDb = await _dbContext.Goals
            .Include(g => g.User)
            .FirstOrDefaultAsync(g => g.Id == goalId);
        
        if (goalDb == null)
        {
            throw new PersonalizedException("Goal not found");
        }
        
        return await _dbContext.Contributions
            .Include(c => c.User)
            .Where(c => c.Goal.Id == goalId)
            .ToListAsync();
    }
}
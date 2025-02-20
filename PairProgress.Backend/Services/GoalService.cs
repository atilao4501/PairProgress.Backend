using Microsoft.EntityFrameworkCore;
using PairProgress.Backend.Data;
using PairProgress.Backend.Models;
using PairProgress.Backend.Services.Interfaces;

namespace PairProgress.Backend.Services;

public class GoalService : IGoalService
{
    private readonly AppDbContext _dbContext;

    public GoalService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task CreateGoal(CreateGoalInput goalInput, string userCode)
    {
        
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserCode == userCode);
        if (user == null)
        {
            throw new PersonalizedException("User not found");
        }

        var goalDb = await _dbContext.Goals.FirstOrDefaultAsync(g => g.Name == goalInput.Name);
        if (goalDb != null)
        {
            throw new PersonalizedException("Goal name already exists");
        }
        
        var goal = new Goal
        {
            User = user,
            Name = goalInput.Name,
            Description = goalInput.Description,
            TargetAmount = goalInput.TargetAmount,
            CurrentAmount = goalInput.CurrentAmount,
            Date = goalInput.Date,
            RecommendedInvestmentPerMonth = goalInput.RecommendedInvestmentPerMonth
        };

        await _dbContext.Goals.AddAsync(goal);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<Goal>> GetGoalsByUserCode(string userCode)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserCode == userCode);
        if (user == null)
        {
            throw new PersonalizedException("User not found");
        }

        var userDuo = await _dbContext.UserDuos
            .Include(ud => ud.User1) 
            .Include(ud => ud.User2) 
            .FirstOrDefaultAsync(ud => ud.User1Code == userCode || ud.User2Code == userCode);
        
        if (userDuo != null)
        {
            return await _dbContext.Goals.Where(g => g.User == userDuo.User1 || g.User == userDuo.User2).ToListAsync();
        }
        
         return await _dbContext.Goals.Where(g => g.User == user).ToListAsync();
    }

    public async Task EditGoalById(UpdateGoalInput goalInput)
    {
        var goal = await _dbContext.Goals.FirstOrDefaultAsync(g => g.Id == goalInput.Id);
        if (goal == null)
        {
            throw new PersonalizedException("Goal not found");
        }

        goal.Name = goalInput.Name;
        goal.Description = goalInput.Description;
        goal.TargetAmount = goalInput.TargetAmount;
        goal.CurrentAmount = goalInput.CurrentAmount;
        goal.Date = goalInput.Date;
        goal.RecommendedInvestmentPerMonth = goalInput.RecommendedInvestmentPerMonth;

        await _dbContext.SaveChangesAsync();
    }

    public async Task<Goal> GetGoalById(int goalId)
    {
        var goal = await _dbContext.Goals
            .Include(g => g.User)
            .FirstOrDefaultAsync(g => g.Id == goalId);

        if (goal == null)
        {
            throw new PersonalizedException("Goal not found");
        }

        return goal;
    }
}
using PairProgress.Backend.Models;

namespace PairProgress.Backend.Services.Interfaces;

public interface IGoalService
{
    public Task CreateGoal(CreateGoalInput goalInput, string userCode);
    public Task<List<Goal>> GetGoalsByUserCode(string userCode);
    public Task EditGoalById(UpdateGoalInput goalInput);
    public Task<Goal> GetGoalById(int goalId);
    public Task RemoveGoalById(int goalId);
}
using PairProgress.Backend.Models;

namespace PairProgress.Backend.Services.Interfaces;

public interface IContributionService
{
    Task AddContributionAsync(CreateContributionInput contributionInput, string userCode);
    Task RemoveContributionAsync(int contributionId);
    Task<IEnumerable<Contribution>> GetContributionsByGoalAsync(int goalId);
}
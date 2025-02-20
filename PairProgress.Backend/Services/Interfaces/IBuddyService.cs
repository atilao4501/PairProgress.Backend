using PairProgress.Backend.Models.Enum;

namespace PairProgress.Backend.Services.Interfaces;

public interface IBuddyService
{
    public Task<BuddyMoodEnum> GetBuddyMoodForUserAsync(string userCode);
}
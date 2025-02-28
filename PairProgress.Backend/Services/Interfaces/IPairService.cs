using PairProgress.Backend.Models;

namespace PairProgress.Backend.Services.Interfaces;

public interface IPairService
{
    public Task<bool> CreatePair(string userCode1, string userCode2);
    public Task<bool> DeletePair(string userCode);
    public Task<UserDuo> GetPairByUserCode(string userCode);
}
using PairProgress.Backend.Models;

namespace PairProgress.Backend.Services.Interfaces;

public interface IUserService
{
    public Task<UserReturn> GetUserByCode(string userCode);
}
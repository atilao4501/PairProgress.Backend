using PairProgress.Backend.Models;

namespace PairProgress.Backend.Services.Interfaces;

public interface IUserService
{
    public Task<GetUserOutput> GetUserByCode(string userCode);
    public Task EditUserByCode(UpdateUserInput userInput);
}
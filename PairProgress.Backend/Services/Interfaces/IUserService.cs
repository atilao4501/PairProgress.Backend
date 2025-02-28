using PairProgress.Backend.Models;

namespace PairProgress.Backend.Services.Interfaces;

public interface IUserService
{
    public Task EditUserByCode(UpdateUserInput userInput, string userCode);
    public bool CheckIfUsernameExists(string username);
    public bool CheckIfEmailExists(string email);
    public Task<GetUserOutput> GetUserByUserCode(string userCode);
}
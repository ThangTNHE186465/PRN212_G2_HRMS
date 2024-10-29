using HRM.Models;

namespace HRM.Repositories;

public interface IUserRepository
{
    Task<User> GetByUsernameAsync(string username);

    Task<bool> IsUsernameTakenAsync(string username);
}
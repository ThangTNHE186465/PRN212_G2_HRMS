using HRM.Models;

namespace HRM.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User> GetByUsernameAsync(string username);
    Task<bool> IsUsernameTakenAsync(string username);

    Task<User> GetByUsernameAsync(string username);
    Task<bool> IsUsernameTakenAsync(string username);
}
using HRM.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRM.Repositories.RepositoryImpl;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository() : base(new HrmContext())
    {
    }

    public async Task<User> GetByUsernameAsync(string username)
    {
        return await _dbSet.FirstOrDefaultAsync(u => 
            u.Username.ToLower() == username.ToLower());
    }

    public async Task<bool> IsUsernameTakenAsync(string username)
    {
        return await _dbSet.AnyAsync(u => 
            u.Username.ToLower() == username.ToLower());
    }
}
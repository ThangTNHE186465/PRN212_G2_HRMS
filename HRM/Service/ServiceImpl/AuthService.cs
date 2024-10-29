using System.IO;
using HRM.Models;
using HRM.Repositories;
using HRM.Repositories.RepositoryImpl;
using HRM.Utils;
using Microsoft.EntityFrameworkCore;

namespace HRM.Service.ServiceImpl;

public class AuthService : IAuthService
{
    private readonly IBaseRepository<User> _userRepository;

    public AuthService()
    {
        _userRepository = new UserRepository();
    }
    
    public async Task<User> LoginAsync(string username, string password)
    {
        User user = await _userRepository
            .GetQueryable()
            .FirstOrDefaultAsync(u => u.Username == username);
        if (user == null)
        {
            // user = await _userRepository.AddAsync(new User
            // {
            //     Username = username,
            //     PasswordHash = password,
            //     Role = "User"
            // });
            // return user;
            throw new UnauthorizedAccessException("Tài khoản không tồn tại! Vui lòng thử lại.");
        }
        if (user.Password != (password))
        {
            throw new UnauthorizedAccessException("Mật khẩu không đúng! Vui lòng thử lại.");
        }
        user.LastLoginDate = DateTime.Now;
        await _userRepository.UpdateAsync(user);
        return user;
    }

    public Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
    {
        User user = _userRepository.GetByIdAsync(userId).Result;
        if (user.Password != (currentPassword))
        // if (user.PasswordHash != PasswordUtils.HashPassword(currentPassword))
        {
            throw new UnauthorizedAccessException("Mật khẩu không khớp! Vui lòng thử lại.");
        }
        user.Password = PasswordUtils.HashPassword(newPassword);
        _userRepository.UpdateAsync(user);
        return Task.FromResult(true);
    }

    public Task LogoutAsync(int userId)
    {
        return Task.CompletedTask;
    }

    public Task SaveUserSessionAsync(User user)
    {
        File.WriteAllText("UserSession.txt", user.Username);
        return Task.CompletedTask;
    }
}
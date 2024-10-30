using HRM.Models;

namespace HRM.Service;

public interface IAuthService
{
    Task<User?> LoginAsync(string username, string password);
    Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
    Task LogoutAsync(int userId);
    Task SaveUserSessionAsync(User user);
}
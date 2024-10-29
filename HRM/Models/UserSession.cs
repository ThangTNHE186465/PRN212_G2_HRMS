namespace HRM.Models;

public class UserSession
{
    private static UserSession _instance;
    public static UserSession Instance => _instance ??= new UserSession();

    public User User { get; private set; }

    private UserSession() { }

    public void SetUser(User user)
    {
        User = user;
    }  
}
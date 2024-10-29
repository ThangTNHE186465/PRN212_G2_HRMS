using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HRM.Service;
using HRM.Service.ServiceImpl;
using HRM.Views;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;

namespace HRM.ViewModels;

public partial class LoginViewModel : ObservableValidator
{
    private readonly IAuthService _authService;

    [ObservableProperty] [Required(ErrorMessage = "Username is required")]
    private string username;

    [ObservableProperty] [Required(ErrorMessage = "Password is required")]
    private string password;

    [ObservableProperty] private string errorMessage;

    public LoginViewModel()
    {
        _authService = new AuthService();
        LoginCommand = new RelayCommand(ExecuteLogin);
    }

    public ICommand LoginCommand { get; }

    private async void ExecuteLogin()
    {
        try
        {
            ErrorMessage = string.Empty;

            var result = await _authService.LoginAsync(Username, Password);
            LoginView loginView = new LoginView();
            loginView.Close();
            loginView.Hide();
            switch (result.Role)
            {
                case "Admin":
                    AdminWindow adminWindow = new AdminWindow();
                    adminWindow.Show();
                    break;
                case "User":
                    UserWindow userWindow = new UserWindow();
                    userWindow.Show();
                    break;
                default:
                    ErrorMessage = "Invalid role";
                    break;
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }

    private bool CanExecuteLogin() =>
        !string.IsNullOrEmpty(Username) &&
        !string.IsNullOrEmpty(Password);
}
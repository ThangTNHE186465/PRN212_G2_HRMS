using System.ComponentModel.DataAnnotations;
using System.Windows.Controls;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HRM.Models;
using HRM.Service;
using HRM.Service.ServiceImpl;
using HRM.Views;

namespace HRM.ViewModels;

public partial class LoginViewModel : ObservableValidator
{
    private LoginView _loginView;
    private readonly IAuthService _authService;

    [ObservableProperty]
    [Required(ErrorMessage = "Username is required")]
    private string username;

    [ObservableProperty]
    [Required(ErrorMessage = "Password is required")]
    private string password;

    [ObservableProperty] private string errorMessage;

    public LoginViewModel(LoginView loginView)
    {
        _loginView = loginView;
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
            if (result != null)
            {
                UserSession.Instance.SetUser(result);
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
                _loginView.Close();
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


    private LoginView _loginView;
    private readonly IAuthService _authService;

    [ObservableProperty]
    [Required(ErrorMessage = "Username is required")]
    private string username;

    [ObservableProperty]
    [Required(ErrorMessage = "Password is required")]
    private string password;

    [ObservableProperty] private string errorMessage;

    public LoginViewModel(LoginView loginView)
    {
        _loginView = loginView;
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
            if (result != null)
            {
                UserSession.Instance.SetUser(result);
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
                _loginView.Close();
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
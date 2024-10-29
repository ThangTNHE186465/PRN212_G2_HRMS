using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HRM.Service;
using HRM.Service.ServiceImpl;

namespace HRM.ViewModels.Admin;

public partial class AdminViewModel : ObservableObject
{
    private readonly INavigationService _navigationService;
    private readonly IAuthService _authService;
    // private readonly IUserService _userService;

    [ObservableProperty] private string _adminName;
    [ObservableProperty] private BitmapImage _adminAvatar;
    [ObservableProperty] private bool _isLoading;

    public AdminViewModel()
    {
    }
    public AdminViewModel(Frame frame)
    {
        _navigationService = new NavigationService(frame);
        _authService = new AuthService();
        // _userService = userService;

        // Initialize commands
        NavigateCommand = new RelayCommand<string>(ExecuteNavigate);
        ProfileCommand = new RelayCommand(ExecuteProfile);
        LogoutCommand = new RelayCommand(ExecuteLogout);

        // Load admin info
        // LoadAdminInfo();
    }

    public ICommand NavigateCommand { get; }
    public ICommand ProfileCommand { get; }
    public ICommand LogoutCommand { get; }

    private void ExecuteNavigate(string? destination)
    {
        switch (destination)
        {
            case "EmployeeManagement":
                _navigationService.NavigateTo("EmployeeListView");
                break;
            // case "DepartmentManagement":
            //     _navigationService.NavigateTo();
            //     break;
            // case "PayrollManagement":
            //     _navigationService.NavigateTo();
            //     break;
            // case "Statistics":
            //     _navigationService.NavigateTo();
            //     break;
        }
    }

    private void ExecuteProfile()
    {
        // _navigationService.NavigateTo<AdminProfileView>();
    }

    private async void ExecuteLogout()
    {
        try
        {
            IsLoading = true;
            await _authService.LogoutAsync(1);
            _navigationService.NavigateToLogin();
        }
        catch (Exception ex)
        {
            // Handle logout error
            ShowError("Đăng xuất thất bại", ex.Message);
        }
        finally
        {
            IsLoading = false;
        }
    }

    // private async void LoadAdminInfo()
    // {
    //     try
    //     {
    //         IsLoading = true;
    //         var currentUser = await _userService.GetCurrentUserAsync();
    //         AdminName = currentUser.FullName;
    //         AdminAvatar = await LoadAvatarImage(currentUser.AvatarUrl);
    //     }
    //     catch (Exception ex)
    //     {
    //         // Handle loading error
    //         ShowError("Lỗi tải thông tin", ex.Message);
    //     }
    //     finally
    //     {
    //         IsLoading = false;
    //     }
    // }

    private async Task<BitmapImage> LoadAvatarImage(string avatarUrl)
    {
        if (string.IsNullOrEmpty(avatarUrl))
            return new BitmapImage(new Uri("pack://application:,,,/Assets/default-avatar.png"));

        try
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(avatarUrl);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            return bitmap;
        }
        catch
        {
            return new BitmapImage(new Uri("pack://application:,,,/Assets/default-avatar.png"));
        }
    }

    private void ShowError(string title, string message)
    {
        // Implement error showing logic (e.g., using dialog service)
        MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
    }
}
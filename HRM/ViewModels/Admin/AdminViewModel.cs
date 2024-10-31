using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HRM.Models;
using HRM.Service;
using HRM.Service.ServiceImpl;

namespace HRM.ViewModels;

public partial class AdminViewModel : ObservableObject
{
    private readonly INavigationService _navigationService;
    private readonly IAuthService _authService;
    private readonly IEmployeeService _employeeService;

    [ObservableProperty] private string? _adminName;
    [ObservableProperty] private BitmapImage? _adminAvatar;
    [ObservableProperty] private bool _isLoading;

    public AdminViewModel(Frame frame)
    {
        _navigationService = new NavigationService(frame);
        _authService = new AuthService();
        _employeeService = new EmployeeService();

        // Initialize commands
        NavigateCommand = new RelayCommand<string>(ExecuteNavigate);
        ProfileCommand = new RelayCommand(ExecuteProfile);

        // Load admin info
        LoadAdminInfo();
    }

    public ICommand NavigateCommand { get; }
    public ICommand ProfileCommand { get; }
    public ICommand LogoutCommand { get; set; }

    private void ExecuteNavigate(string? destination)
    {
        switch (destination)
        {
            case "Home":
                _navigationService.NavigateTo("AdminDashboard");
                break;
            case "EmployeeManagement":
                _navigationService.NavigateTo("EmployeeListView");
                break;
            case "DepartmentManagement":
                _navigationService.NavigateTo("DepartmentListView");
                break;
            case "SalaryManagement":
                _navigationService.NavigateTo("SalaryManagementView");
                break;
            case "Statistics":
                _navigationService.NavigateTo("ReportView");
                break;
        }
    }

    private void ExecuteProfile()
    {
        _navigationService.NavigateTo("UserProfileView");
    }

    private async void LoadAdminInfo()
    {
        try
        {
            IsLoading = true;
            var currentEmp = UserSession.Instance.Employee;
            AdminName = currentEmp.FullName;
            AdminAvatar = await LoadAvatarImage(currentEmp.PhotoPath);
        }
        catch (Exception ex)
        {
            // Handle loading error
            ShowError("Lỗi tải thông tin", ex.Message);
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task<BitmapImage?> LoadAvatarImage(string avatarUrl)
    {
        if (string.IsNullOrEmpty(avatarUrl))
            return new BitmapImage(new Uri("pack://application:,,,/Assets/avatar/default.jpg"));

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
            return new BitmapImage(new Uri("pack://application:,,,/Assets/avatar/default.jpg"));
        }
    }

    private void ShowError(string title, string message)
    {
        // Implement error showing logic (e.g., using dialog service)
        MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
    }
}
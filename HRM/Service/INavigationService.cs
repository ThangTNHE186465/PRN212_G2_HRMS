using System.Windows.Navigation;

namespace HRM.Service;

public interface INavigationService
{
    // Điều hướng đến view mới
    void NavigateTo(string viewName, object parameter = null);
        
    // Quay lại view trước đó
    void GoBack();
        
    // Kiểm tra có thể quay lại không
    bool CanGoBack { get; }
        
    // Lưu trữ tham số điều hướng
    object Parameter { get; }
        
    // Event khi điều hướng thay đổi
    event EventHandler<NavigationEventArgs> Navigated;
    void NavigateToLogin();
}
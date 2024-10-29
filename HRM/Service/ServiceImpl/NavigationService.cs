using System.Windows.Controls;
using System.Windows.Navigation;
using HRM.Views;

namespace HRM.Service.ServiceImpl;

public class NavigationService : INavigationService
{
    private readonly Frame _frame;
    private readonly Dictionary<string, Type> _pageMap;
    private object _parameter;

    public NavigationService(Frame frame)
    {
        _frame = frame ?? throw new ArgumentNullException(nameof(frame));
        _frame.Navigated += Frame_Navigated;

        // Đăng ký các pages
        _pageMap = new Dictionary<string, Type>
        {
            { "LoginView", typeof(LoginView) },
            { "EmployeeListView", typeof(EmployeeListView) },
            { "EmployeeDetailView", typeof(EmployeeDetailView) },
        };
    }

    public bool CanGoBack => _frame.CanGoBack;

    public object Parameter => _parameter;

    public event EventHandler<NavigationEventArgs> Navigated;

    public void GoBack()
    {
        if (CanGoBack)
        {
            _frame.GoBack();
        }
    }

    public void NavigateTo(string viewName, object parameter = null)
    {
        if (!_pageMap.ContainsKey(viewName))
        {
            throw new ArgumentException($"Page {viewName} not found in page map.");
        }

        _parameter = parameter;
        var pageType = _pageMap[viewName];

        // Tạo instance của page
        var page = Activator.CreateInstance(pageType);

        // Thực hiện điều hướng
        _frame.Navigate(page, parameter);
    }

    public void NavigateToLogin()
    {
        NavigateTo("LoginView");
    }

private void Frame_Navigated(object sender, NavigationEventArgs e)
    {
        Navigated?.Invoke(this, e);
    }
}
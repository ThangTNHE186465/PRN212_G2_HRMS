using System.Windows.Controls;
using System.Windows.Navigation;
using HRM.Models;
using HRM.Repositories.RepositoryImpl;
using HRM.Views;
using Microsoft.Extensions.Logging;

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
    public EmployeeService()
    {
        _employeeRepository = new EmployeeRepository(new HrmContext());
        _activityLogService = new ActivityLogService();
        _logger = new Logger<EmployeeService>(new LoggerFactory());
    }

    public async Task<Employee?> GetByIdAsync(int id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);
        if (employee == null)
        {
            throw new KeyNotFoundException($"Employee with ID {id} not found");
        }
        return employee;
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
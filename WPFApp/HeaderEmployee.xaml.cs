using BusinessObjects;
using DataAccessObjects;
using Repositories;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace WPFApp
{
    public partial class HeaderEmployee : UserControl, INotifyPropertyChanged
    {
        private string _currentWindowName;
        private readonly EmployeeRepository _employeeRepository;
        private readonly LeaveRequestRepository leaveRequestRepository;
        public string CurrentWindowName
        {
            get => _currentWindowName;
            set
            {
                _currentWindowName = value;
                OnPropertyChanged();
            }
        }

        public HeaderEmployee()
        {
            InitializeComponent();
            DataContext = this;
            _employeeRepository = new EmployeeRepository(new EmployeeDAO(new FuhrmContext())); // Initialize the repository
            Loaded += SideMenuEmployee_Loaded;
        }


        private void SideMenuEmployee_Loaded(object sender, RoutedEventArgs e)
        {
            SetInitialWindowName();
        }

        private void SetInitialWindowName()
        {
            Window currentWindow = Window.GetWindow(this);
            if (currentWindow != null)
            {
                CurrentWindowName = currentWindow.Title; // Set the window title as the initial window name
            }
            else
            {
                CurrentWindowName = "Unknown Window"; // Fallback value if the window is not found
            }
        }


        private void NavigateButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                Window currentWindow = Window.GetWindow(this);

                switch (button.Content.ToString())
                {
                    case "Trang chủ":
                        var homeView = new HomeEmployee();
                        homeView.Show();
                        currentWindow.Close();
                        break;
                    case "Điểm danh":
                        var takeAttendanceView = new TakeAttendance(SessionManager.CurrentAccount.AccountId);
                        takeAttendanceView.Show();
                        currentWindow.Close();
                        break;
                    case "Thông báo":
                        var notificationWindow = new NotificationWindow(SessionManager.CurrentAccount.AccountId, new NotificationRepository(), new EmployeeRepository(new EmployeeDAO(new FuhrmContext())));
                        notificationWindow.Show();
                        currentWindow.Close();
                        break;
                    case "Xin nghỉ phép":
                        var employee = _employeeRepository.GetEmployeeByAccountId(SessionManager.CurrentAccount.AccountId);
                        if (employee != null)
                        {
                            var currentEmployee = new MainWindow(employee.EmployeeId);
                            currentEmployee.Show();
                            currentWindow.Close();
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy nhân viên.");
                        }
                        break;

                    default:
                        break;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            // Clear the session
            SessionManager.CurrentAccount = null;

            // Redirect to login screen
            LoginScreen loginScreen = new LoginScreen();
            loginScreen.Show();

            // Close the current window
            Window.GetWindow(this).Close();
        }
    }
}

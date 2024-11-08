using BusinessObjects;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using DataAccessObjects;
using Repositories;

namespace WPFApp
{
    /// <summary>
    /// Interaction logic for SideMenu.xaml
    /// </summary>
    public partial class HeaderAdmin : UserControl, INotifyPropertyChanged
    {
        private string _currentWindowName;
        private Employee _currentEmployee;
        public string CurrentWindowName
        {
            get => _currentWindowName;
            set
            {
                _currentWindowName = value;
                OnPropertyChanged();
            }
        }
        // Thêm constructor nhận Employee
        
        public HeaderAdmin()
        {
            InitializeComponent();
            DataContext = this;
            Loaded += SideMenu_Loaded;
        }

        private void SideMenu_Loaded(object sender, RoutedEventArgs e)
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
                    case "Home":
                        var homeView = new AdminDashboard();
                        homeView.Show();
                        currentWindow.Close();
                        break;
                    case "Account":
                        var account = new AccountManagement();
                        account.Show();
                        currentWindow.Close();
                        break;
                    case "Employee":
                        var employeeView = new EmployeeWindow();
                        employeeView.Show();
                        currentWindow.Close();
                        break;
                    case "Department":
                        var departmentView = new DepartmentManagement();
                        departmentView.Show();
                        currentWindow.Close();
                        break;
                    case "Position":
                        var position = new PositionManagement();
                        position.Show();
                        currentWindow.Close();
                        break;
                    case "Attendance":
                        var attendanceView = new AttendanceView();
                        attendanceView.Show();
                        currentWindow.Close();
                        break;
                    case "Salary":
                        var salaryView = new SalaryView();
                        salaryView.Show();
                        currentWindow.Close();
                        break;
                    case "Absent Request":
                        var leaveView = new LeaveRequestView();
                        leaveView.Show();
                        currentWindow.Close();
                        break;
                    case "Notification":
                        var notificationWindow = new NotificationManagement();
                        notificationWindow.Show();
                        currentWindow.Close();
                        break;
                    default:
                        break;
                }
            }
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

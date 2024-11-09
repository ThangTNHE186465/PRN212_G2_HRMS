using BusinessObjects;
using DataAccessObjects;
using Repositories;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace WPFApp
{
    public partial class HeaderEmployee : UserControl, INotifyPropertyChanged
    {
        private string _currentWindowName;
        private readonly EmployeeRepository _employeeRepository;

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
            _employeeRepository = new EmployeeRepository(new EmployeeDAO(new FuhrmContext()));
            Loaded += SideMenuEmployee_Loaded;

            LoadEmployeeAvatar();
        }

        private void LoadEmployeeAvatar()
        {
            if (SessionManager.CurrentAccount == null)
            {
                MessageBox.Show("Không có thông tin tài khoản hiện tại.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var currentEmployee = _employeeRepository.GetEmployeeByAccountId(SessionManager.CurrentAccount.AccountId);
            if (currentEmployee != null && !string.IsNullOrEmpty(currentEmployee.ProfilePicture))
            {
                try
                {
                    ProfileAvatar.Source = new BitmapImage(new Uri(currentEmployee.ProfilePicture, UriKind.RelativeOrAbsolute));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi khi tải ảnh đại diện: {ex.Message}");
                    ProfileAvatar.Source = new BitmapImage(new Uri("/Resources/default_avatar.png", UriKind.Relative));
                }
            }
            else
            {
                ProfileAvatar.Source = new BitmapImage(new Uri("/Resources/default_avatar.png", UriKind.Relative));
            }
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
                CurrentWindowName = currentWindow.Title; 
            }
            else
            {
                CurrentWindowName = "Unknown Window"; 
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
                        var homeView = new HomeEmployee();
                        homeView.Show();
                        currentWindow.Close();
                        break;
                    case "Take Attendance":
                        var takeAttendanceView = new TakeAttendance(SessionManager.CurrentAccount.AccountId);
                        takeAttendanceView.Show();
                        currentWindow.Close();
                        break;
                    case "Notification":
                        var notificationWindow = new NotificationWindow(SessionManager.CurrentAccount.AccountId, new NotificationRepository(), new EmployeeRepository(new EmployeeDAO(new FuhrmContext())));
                        notificationWindow.Show();
                        currentWindow.Close();
                        break;
                    case "Absent Request":
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
            SessionManager.CurrentAccount = null;

            LoginScreen loginScreen = new LoginScreen();
            loginScreen.Show();

            Window.GetWindow(this).Close();
        }
    }
}
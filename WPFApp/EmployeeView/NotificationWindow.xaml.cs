using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using BusinessObjects;
using Repositories;

namespace WPFApp
{
    public partial class NotificationWindow : Window
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly int _accountId;
        public Employee LoggedInEmployee { get; set; }
        public ObservableCollection<Notification> Notifications { get; set; }

        public NotificationWindow(int accountId, INotificationRepository notificationRepository, IEmployeeRepository employeeRepository)
        {
            InitializeComponent();
            _accountId = accountId;
            _notificationRepository = notificationRepository;
            _employeeRepository = employeeRepository;
            LoadEmployeeDetails();
        }

        private void LoadEmployeeDetails()
        {
            var employee = _employeeRepository.GetAllEmployees().FirstOrDefault(e => e.AccountId == _accountId);
            if (employee != null)
            {
                LoggedInEmployee = employee;
                LoadNotifications();
            }
            else
            {
                MessageBox.Show("Không thể tìm thấy nhân viên.", "Đã xảy ra lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
        }

        private void LoadNotifications()
        {
            var notifications = _notificationRepository.GetNotificationsByDepartmentId(LoggedInEmployee.DepartmentId);
            Notifications = new ObservableCollection<Notification>(notifications);
            NotificationListView.ItemsSource = Notifications;
        }
    }
}

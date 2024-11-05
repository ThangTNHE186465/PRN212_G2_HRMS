using System.Windows;
using System.Windows.Controls;
using Repositories;
using BusinessObjects;
using System.Collections.Generic;
using System.Linq;

namespace WPFApp
{
    public partial class NotificationManagement : Window
    {
        private readonly NotificationRepository _notificationRepository;
        private readonly DepartmentRepository _departmentRepository;

        public NotificationManagement()
        {
            InitializeComponent();
            _notificationRepository = new NotificationRepository();
            _departmentRepository = new DepartmentRepository();
            LoadDepartments();
            LoadNotifications();
        }

        private void LoadNotifications()
        {
            List<Notification> notifications = _notificationRepository.GetNotifications();
            NotificationDataGrid.ItemsSource = notifications;
        }

        private void LoadDepartments()
        {
            DepartmentComboBox.ItemsSource = _departmentRepository.GetDepartments();
        }

        private void NotificationDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (NotificationDataGrid.SelectedItem is Notification selectedNotification)
            {
                DisplayNotificationDetails(selectedNotification);
            }
        }

        private void DisplayNotificationDetails(Notification notification)
        {
            TitleTextBox.Text = notification.Title;
            ContentTextBox.Text = notification.Content;
            DepartmentComboBox.SelectedValue = notification.DepartmentId;
            CreatedDatePicker.SelectedDate = notification.CreatedDate;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var notification = new Notification()
            {
                Title = TitleTextBox.Text,
                Content = ContentTextBox.Text,
                DepartmentId = (DepartmentComboBox.SelectedItem as Department).DepartmentId,
                CreatedDate = CreatedDatePicker.SelectedDate ?? DateTime.Now
            };
            _notificationRepository.AddNotification(notification);
            LoadNotifications();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (NotificationDataGrid.SelectedItem is Notification selectedNotification)
            {
                selectedNotification.Title = TitleTextBox.Text;
                selectedNotification.Content = ContentTextBox.Text;
                selectedNotification.DepartmentId = (DepartmentComboBox.SelectedItem as Department).DepartmentId;
                selectedNotification.CreatedDate = CreatedDatePicker.SelectedDate ?? DateTime.Now;
                _notificationRepository.UpdateNotification(selectedNotification);
                LoadNotifications();
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (NotificationDataGrid.SelectedItem is Notification selectedNotification)
            {
                _notificationRepository.RemoveNotification(selectedNotification);
                LoadNotifications();
            }
        }
    }
}

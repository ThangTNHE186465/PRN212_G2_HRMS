using Repositories;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using BusinessObjects;
using DataAccessObjects;

namespace WPFApp
{
    public partial class TakeAttendance : Window
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly int _accountId;

        public TakeAttendance(int accountId)
        {
            InitializeComponent();
            var context = new FuhrmContext();
            _employeeRepository = new EmployeeRepository(new EmployeeDAO(context));
            _attendanceRepository = new AttendanceRepository();
            _accountId = accountId;
            LoadEmployeeDetails();
        }

        private void LoadEmployeeDetails()
        {
            var employee = _employeeRepository.GetAllEmployees().FirstOrDefault(e => e.AccountId == _accountId);
            if (employee != null)
            {
                EmployeeIdTextBox.Text = employee.EmployeeId.ToString();
                EmployeeNameTextBox.Text = employee.FullName;
                AttendanceDateTextBox.Text = DateTime.Now.ToString("yyyy-MM-dd");
                StatusTextBox.Text = "Present";
            }
            else
            {
                MessageBox.Show("Không tìm thấy nhân viên.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
        }

        private void SaveAttendanceButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var attendance = new Attendance
                {
                    EmployeeId = int.Parse(EmployeeIdTextBox.Text),
                    Date = DateOnly.FromDateTime(DateTime.Now),
                    Status = "Present"
                };
                _attendanceRepository.AddAttendance(attendance);
                MessageBox.Show("Chấm công thành công.", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                HomeEmployee homeEmployee = new HomeEmployee();
                homeEmployee.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi chấm công: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void CheckoutButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _attendanceRepository.CheckoutAttendance(int.Parse(EmployeeIdTextBox.Text));
                MessageBox.Show("Checkout thành công.", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                HomeEmployee homeEmployee = new HomeEmployee();
                homeEmployee.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi checkout: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
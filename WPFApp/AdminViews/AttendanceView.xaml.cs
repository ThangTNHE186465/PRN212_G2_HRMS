using Repositories;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using BusinessObjects;
using System;
using System.Globalization;
using BusinessObjects;

namespace WPFApp
{
    public partial class AttendanceView : Window
    {
        private readonly IAttendanceRepository _attendanceRepository;

        public AttendanceView()
        {
            InitializeComponent();
            _attendanceRepository = new AttendanceRepository();
            LoadAttendances();
        }

        private void LoadAttendances()
        {
            var attendances = _attendanceRepository.GetAttendances();
            AttendanceDataGrid.ItemsSource = attendances;
        }

        private void AttendanceDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AttendanceDataGrid.SelectedItem is Attendance selectedAttendance)
            {
                AttendanceIdTextBox.Text = selectedAttendance.AttendanceId.ToString();
                EmployeeNameTextBox.Text = selectedAttendance.Employee.FullName;
                EmployeeIdTextBox.Text = selectedAttendance.EmployeeId.ToString();
                AttendanceDateTextBox.Text = selectedAttendance.Date.ToString("yyyy-MM-dd");
                StatusTextBox.Text = selectedAttendance.Status;
            }
            else
            {
                ClearAttendanceFields();
            }
        }

        private void ClearAttendanceFields()
        {
            AttendanceIdTextBox.Text = string.Empty;
            EmployeeNameTextBox.Text = string.Empty;
            EmployeeIdTextBox.Text = string.Empty;
            AttendanceDateTextBox.Text = string.Empty;
            StatusTextBox.Text = string.Empty;
        }
        private void AddAttendanceButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var attendance = new Attendance
                {
                    EmployeeId = int.Parse(EmployeeIdTextBox.Text),
                    Date = DateOnly.ParseExact(AttendanceDateTextBox.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    Status = StatusTextBox.Text
                };
                _attendanceRepository.AddAttendance(attendance);
                LoadAttendances();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding attendance: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditAttendanceButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (AttendanceDataGrid.SelectedItem is Attendance selectedAttendance)
                {
                    selectedAttendance.Date = DateOnly.ParseExact(AttendanceDateTextBox.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    selectedAttendance.Status = StatusTextBox.Text;
                    _attendanceRepository.UpdateAttendance(selectedAttendance);
                    LoadAttendances();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error editing attendance: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteAttendanceButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (AttendanceDataGrid.SelectedItem is Attendance selectedAttendance)
                {
                    _attendanceRepository.RemoveAttendance(selectedAttendance);
                    LoadAttendances();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting attendance: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

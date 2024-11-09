using DataAccessObjects;
using Repositories;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BusinessObjects;
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using BusinessObjects;
using LiveCharts.Wpf;
using LiveCharts;

namespace WPFApp
{
    public partial class AdminDashboard : Window
    {
        private readonly IEmployeeRepository _employeeRepository;

        public class DepartmentStats
        {
            public string DepartmentName { get; set; }
            public int EmployeeCount { get; set; }
            public string Percentage { get; set; }
            public double ProgressValue { get; set; }
        }

        public AdminDashboard()
        {
            InitializeComponent();
            
            var context = new FuhrmContext();
            var employeeDAO = new EmployeeDAO(context);
            _employeeRepository = new EmployeeRepository(employeeDAO);

            LoadDashboardData();
        }

        private void LoadDashboardData()
        {
            try
            {
                LoadTotalEmployees();
                LoadEmployeesByDepartment();
                LoadLeaveRequests();
                LoadSalaryExpense();
                LoadNotifications();
                LoadAttendanceData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra khi tải dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadAttendanceData()
        {
            using (var context = new FuhrmContext())
            {
                AttendanceReportDateTextBlock.Text = $"Date: {DateTime.Now:dd/MM/yyyy}";

                var today = DateOnly.FromDateTime(DateTime.Now);

                var totalEmployees = _employeeRepository.GetAllEmployees().Count();
                var presentCount = context.Attendances.Count(a => a.Date == today && a.Status == "Present");
                var absentCount = totalEmployees - presentCount;

                PresentCountTextBlock.Text = presentCount.ToString();
                AbsentCountTextBlock.Text = absentCount.ToString();
            }
        }

        private void LoadTotalEmployees()
        {
            var totalEmployees = _employeeRepository.GetAllEmployees().Count();
            TotalEmployeesTextBlock.Text = totalEmployees.ToString("N0");
        }

        private void LoadEmployeesByDepartment()
        {
            var context = new FuhrmContext();
            var totalEmployees = context.Employees.Count();

            var employeesByDepartment = context.Employees
                .GroupBy(e => e.Department.DepartmentName)
                .Select(group => new
                {
                    Department = group.Key,
                    Count = group.Count(),
                }).ToList();

            EmployeesByDepartmentPieChart.Series.Clear();

            foreach (var departmentData in employeesByDepartment)
            {
                double percentage = departmentData.Count / (double)totalEmployees * 100;

                var pieSeries = new PieSeries
                {
                    Title = departmentData.Department,
                    Values = new ChartValues<double> { percentage },
                    DataLabels = true,
                    LabelPoint = chartPoint => $"{departmentData.Count} ({chartPoint.Y:F2})%"
                };

                EmployeesByDepartmentPieChart.Series.Add(pieSeries);
            }
        }

        private void LoadLeaveRequests()
        {
            using (var context = new FuhrmContext())
            {
                var pendingCount = context.LeaveRequests.Count(l => l.Status == "Pending");
                PendingLeaveRequestsTextBlock.Text = pendingCount.ToString("N0");
            }
        }

        private void LoadSalaryExpense()
        {
            using (var context = new FuhrmContext())
            {
                var totalSalary = context.Employees.Sum(e => e.Salary);
                TotalSalaryExpenseTextBlock.Text = totalSalary.ToString("N0") + " VNĐ";
            }
        }

        private void LoadNotifications()
        {
            using (var context = new FuhrmContext())
            {
                var recentNotifications = context.Notifications
                    .OrderByDescending(n => n.CreatedDate)
                    .Take(10)
                    .Select(n => new
                    {
                        Content = n.Content,
                        Date = n.CreatedDate.ToString("dd/MM/yyyy HH:mm"),
                        IsNew = (DateTime.Now - n.CreatedDate).TotalDays <= 1
                    })
                    .ToList();

                NotificationsItemsControl.ItemsSource = recentNotifications;
            }
        }
    }
}
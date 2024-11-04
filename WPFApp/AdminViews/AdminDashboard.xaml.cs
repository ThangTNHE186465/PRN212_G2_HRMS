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

namespace WPFApp
{
    public partial class AdminDashboard : Window
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly DispatcherTimer _refreshTimer;
        private ObservableCollection<DepartmentStats> _departmentStats;

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
            
            _departmentStats = new ObservableCollection<DepartmentStats>();
            EmployeesByDepartmentItemsControl.ItemsSource = _departmentStats;
            
            _refreshTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMinutes(5)
            };
            _refreshTimer.Tick += (s, e) => LoadDashboardData();
            _refreshTimer.Start();

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
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra khi tải dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
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
            var employeesByDepartment = context.Employees
                .GroupBy(e => e.Department.DepartmentName)
                .Select(group => new
                {
                    Department = group.Key,
                    Count = group.Count(),
                }).ToList();

            EmployeesByDepartmentItemsControl.ItemsSource = employeesByDepartment
                .Select(d => new
                {
                    DepartmentName = d.Department,
                    EmployeeCount = d.Count,
                    Percentage = $"{(d.Count / (double)context.Employees.Count() * 100):F2}%",
                    ProgressValue = d.Count / (double)context.Employees.Count()
                }).ToList();
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
        
        protected override void OnClosed(EventArgs e)
        {
            _refreshTimer.Stop();
            base.OnClosed(e);
        }

    }
}
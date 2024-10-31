using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HRM.Models;

namespace HRM.ViewModels
{
    partial class AdminDashBoardViewModel : ObservableObject
    {
        [ObservableProperty] private int totalEmployees;

        [ObservableProperty] private int totalDepartments;

        [ObservableProperty] private int newEmployees;


        // Quick Action Commands
        public ICommand AddEmployeeCommand { get; }
        public ICommand ManageDepartmentsCommand { get; }
        public ICommand PayrollManagementCommand { get; }

        public ObservableCollection<ActivityLog> RecentActivities { get; }

        public AdminDashBoardViewModel()
        {
            TotalEmployees = 120;
            TotalDepartments = 8;
            NewEmployees = 5;

            // Initialize commands
            AddEmployeeCommand = new RelayCommand(AddEmployee);
            ManageDepartmentsCommand = new RelayCommand(ManageDepartments);
            PayrollManagementCommand = new RelayCommand(PayrollManagement);

            // Initialize Recent Activities
            RecentActivities =
            [
                new ActivityLog { Action = "Added new employee John Doe", CreatedDate = DateTime.Now.AddDays(-1) },
                new ActivityLog { Action = "Updated department HR", CreatedDate = DateTime.Now.AddDays(-2) },
                new ActivityLog { Action = "Processed payroll for March", CreatedDate = DateTime.Now.AddDays(-3) }
            ];
        }

        // Command Methods
        private void AddEmployee()
        {
            // Logic to add an employee
        }

        private void ManageDepartments()
        {
            // Logic to manage departments
        }

        private void PayrollManagement()
        {
            // Logic to manage payroll
        }
    }
}
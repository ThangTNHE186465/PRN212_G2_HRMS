using Repositories;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using BusinessObjects;

namespace WPFApp
{
    public partial class SalaryView : Window
    {
        private readonly SalaryRepository _salaryRepository;
        private readonly EmployeeRepository _employeeRepository;

        public SalaryView()
        {
            InitializeComponent();
            _salaryRepository = new SalaryRepository();
            _employeeRepository = new EmployeeRepository();
            LoadSalaries();
        }

        private void LoadSalaries()
        {
            var salaries = _salaryRepository.GetSalaries();
            SalaryDataGrid.ItemsSource = salaries;
        }

        private void SalaryDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SalaryDataGrid.SelectedItem is Salary selectedSalary)
            {
                SalaryIdTextBox.Text = selectedSalary.SalaryId.ToString();
                EmployeeNameTextBox.Text = selectedSalary.Employee.FullName;
                EmployeeIdTextBox.Text = selectedSalary.EmployeeId.ToString();
                BaseSalaryTextBox.Text = selectedSalary.BaseSalary.ToString();
                AllowanceTextBox.Text = selectedSalary.Allowance?.ToString();
                BonusTextBox.Text = selectedSalary.Bonus?.ToString();
                PenaltyTextBox.Text = selectedSalary.Penalty?.ToString();
                PaymentDateTextBox.Text = selectedSalary.PaymentDate.ToString("yyyy-MM-dd");
            }
            else
            {
                ClearSalaryFields();
            }
        }

        private void ClearSalaryFields()
        {
            SalaryIdTextBox.Text = string.Empty;
            EmployeeNameTextBox.Text = string.Empty;
            EmployeeIdTextBox.Text = string.Empty;
            BaseSalaryTextBox.Text = string.Empty;
            AllowanceTextBox.Text = string.Empty;
            BonusTextBox.Text = string.Empty;
            PenaltyTextBox.Text = string.Empty;
            PaymentDateTextBox.Text = string.Empty;
        }

        private void AddSalaryButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var salary = new Salary
                {
                    EmployeeId = int.Parse(EmployeeIdTextBox.Text),
                    BaseSalary = double.Parse(BaseSalaryTextBox.Text),
                    Allowance = string.IsNullOrEmpty(AllowanceTextBox.Text) ? (double?)null : double.Parse(AllowanceTextBox.Text),
                    Bonus = string.IsNullOrEmpty(BonusTextBox.Text) ? (double?)null : double.Parse(BonusTextBox.Text),
                    Penalty = string.IsNullOrEmpty(PenaltyTextBox.Text) ? (double?)null : double.Parse(PenaltyTextBox.Text),
                    PaymentDate = DateOnly.ParseExact(PaymentDateTextBox.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture)
                };
                _salaryRepository.AddSalary(salary);
                var employee = _employeeRepository.GetEmployeeById(salary.EmployeeId);
                employee.Salary = salary.BaseSalary;
                _employeeRepository.UpdateEmployee(employee);
                LoadSalaries();
                MessageBox.Show("Thêm lương thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm lương: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditSalaryButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SalaryDataGrid.SelectedItem is Salary selectedSalary)
                {
                    selectedSalary.BaseSalary = double.Parse(BaseSalaryTextBox.Text);
                    selectedSalary.Allowance = string.IsNullOrEmpty(AllowanceTextBox.Text) ? (double?)null : double.Parse(AllowanceTextBox.Text);
                    selectedSalary.Bonus = string.IsNullOrEmpty(BonusTextBox.Text) ? (double?)null : double.Parse(BonusTextBox.Text);
                    selectedSalary.Penalty = string.IsNullOrEmpty(PenaltyTextBox.Text) ? (double?)null : double.Parse(PenaltyTextBox.Text);
                    selectedSalary.PaymentDate = DateOnly.ParseExact(PaymentDateTextBox.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    _salaryRepository.UpdateSalary(selectedSalary);
                    var employee = _employeeRepository.GetEmployeeById(selectedSalary.EmployeeId);
                    employee.Salary = selectedSalary.BaseSalary;
                    _employeeRepository.UpdateEmployee(employee);
                    LoadSalaries();
                    MessageBox.Show("Chỉnh sửa lương thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi chỉnh sửa lương: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteSalaryButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SalaryDataGrid.SelectedItem is Salary selectedSalary)
                {
                    _salaryRepository.RemoveSalary(selectedSalary);
                    LoadSalaries();
                    MessageBox.Show("Xóa lương thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa lương: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
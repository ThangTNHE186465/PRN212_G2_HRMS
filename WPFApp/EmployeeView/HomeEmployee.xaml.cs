using BusinessObjects;
using DataAccessObjects;
using Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPFApp
{
    public partial class HomeEmployee : Window
    {
        EmployeeRepository employeeRepository;
        public HomeEmployee()
        {
            InitializeComponent();
            FuhrmContext context = new FuhrmContext();
            EmployeeDAO ed = new EmployeeDAO(context);
            employeeRepository = new EmployeeRepository(ed);
            LoadEmployeeData();
        }

        private void LoadEmployeeData()
        {
            var currentAccount = SessionManager.CurrentAccount;
            if (currentAccount != null)
            {
                var employee = employeeRepository.GetEmployeeByAccountId(currentAccount.AccountId);
                if (employee != null)
                {
                    WelcomeTextBlock.Text = $"Hello, {employee.FullName}";
                    NameTextBox.Text = employee.FullName;
                    BirthTextBox.Text = employee.DateOfBirth.ToString("d", CultureInfo.CurrentCulture);
                    GenderTextBox.Text = employee.Gender;
                    AddressTextBox.Text = employee.Address;
                    PhoneTextBox.Text = employee.PhoneNumber;
                    StartTextBox.Text = employee.StartDate.ToString("d", CultureInfo.CurrentCulture);
                    DepartmentTextBox.Text = employee.Department?.DepartmentName ?? "N/A";
                    PositionTextBox.Text = employee.Position?.PositionName ?? "N/A";
                    SalaryTextBox.Text = employee.Salary.ToString( CultureInfo.CurrentCulture);

                    if (!string.IsNullOrEmpty(employee.ProfilePicture))
                    {
                        try
                        {
                            ProfileImage.Source = new BitmapImage(new Uri(employee.ProfilePicture, UriKind.RelativeOrAbsolute));
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error loading profile picture: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            ProfileImage.Source = null; 
                        }
                    }
                    else
                    {
                        ProfileImage.Source = null; 
                    }
                }
                else
                {
                    MessageBox.Show("Employee data could not be found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                WelcomeTextBlock.Text = "Welcome, employee";
            }
        }

        private void EditProfileButton_Click(object sender, RoutedEventArgs e)
        {
            var currentAccount = SessionManager.CurrentAccount;
            if (currentAccount != null)
            {
                var em = employeeRepository.GetEmployeeByAccountId(currentAccount.AccountId);
                EditProfileWindow editProfileWindow = new EditProfileWindow(em);
                editProfileWindow.Show();
                this.Close(); 
            }
            else
            {
                MessageBox.Show("Không thể chỉnh sửa. Không tìm thấy thông tin tài khoản hiện tại.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}

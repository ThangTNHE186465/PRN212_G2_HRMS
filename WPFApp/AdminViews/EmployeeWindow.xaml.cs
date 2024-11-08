using DataAccessObjects;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BusinessObjects;
using System.Windows.Forms.VisualStyles;
using System.Windows.Media.Imaging;
using Microsoft.Win32; // Add this for OpenFileDialog

namespace WPFApp

{
    /// <summary>
    /// Interaction logic for EmployeeWindow.xaml
    /// </summary>
    public partial class EmployeeWindow : Window
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ISalaryRepository _salaryRepository;

        public EmployeeWindow()
        {
            InitializeComponent();
            var context = new FuhrmContext();
            var employeeDAO = new EmployeeDAO(context);
            _employeeRepository = new EmployeeRepository(employeeDAO);
            _salaryRepository = new SalaryRepository();
            LoadComboBoxData();
            LoadEmployees();
        }

        public void LoadEmployees()
        {
            EmployeeDataGrid.ItemsSource = _employeeRepository.GetAllEmployees();
        }

        private void LoadComboBoxData()
        {
            using (var context = new FuhrmContext())
            {
                DepartmentComboBox.ItemsSource = context.Departments.ToList();
                PositionComboBox.ItemsSource = context.Positions.ToList();
                RoleComboBox.ItemsSource = context.Roles.ToList();


                DepartmentComboBox.SelectedValuePath = "DepartmentId";
                PositionComboBox.SelectedValuePath = "PositionId";
                RoleComboBox.SelectedValuePath = "RoleId";
            }
        }

        private void DisplayEmployeeDetails(Employee employee)
        {
            EmployeeIdTextBox.Text = employee.EmployeeId.ToString();
            FullNameTextBox.Text = employee.FullName;
            DateOfBirthDatePicker.SelectedDate = employee.DateOfBirth;

            GenderComboBox.SelectedItem = GenderComboBox.Items.OfType<ComboBoxItem>()
                .FirstOrDefault(item => item.Content.ToString() == employee.Gender);

            AddressTextBox.Text = employee.Address;
            PhoneNumberTextBox.Text = employee.PhoneNumber;


            DepartmentComboBox.SelectedValue = employee.DepartmentId;
            PositionComboBox.SelectedValue = employee.PositionId;
            RoleComboBox.SelectedValue = employee.Account.RoleId;
            CreateDatePicker.SelectedDate = employee.StartDate;
            SalaryTextBox.Text = employee.Salary.ToString();


            if (!string.IsNullOrEmpty(employee.ProfilePicture))
            {
                try
                {
                    EmployeeImage.Source =
                        new BitmapImage(new Uri(employee.ProfilePicture, UriKind.RelativeOrAbsolute));
                    ProfilePictureUrlTextBlock.Text = employee.ProfilePicture;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    EmployeeImage.Source = null;
                    ProfilePictureUrlTextBlock.Text = string.Empty;
                }
            }
            else
            {
                EmployeeImage.Source = null;
                ProfilePictureUrlTextBlock.Text = string.Empty;
            }
        }

        private void ChooseImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string imageDirectory =
                    System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "Employees");
                if (!System.IO.Directory.Exists(imageDirectory))
                {
                    System.IO.Directory.CreateDirectory(imageDirectory);
                }

                string fileName = System.IO.Path.GetFileName(openFileDialog.FileName);
                string destinationPath = System.IO.Path.Combine(imageDirectory, fileName);

                try
                {
                    System.IO.File.Copy(openFileDialog.FileName, destinationPath, true);


                    EmployeeImage.Source = new BitmapImage(new Uri(destinationPath, UriKind.Absolute));
                    ProfilePictureUrlTextBlock.Text = destinationPath;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Có lỗi xảy ra khi lưu ảnh: {ex.Message}", "Lỗi", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
        }


        private void EmployeeDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EmployeeDataGrid.SelectedItem is Employee selectedEmployee)
            {
                DisplayEmployeeDetails(selectedEmployee);
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            EmployeeIdTextBox.Text = "";
            FullNameTextBox.Text = "";
            DateOfBirthDatePicker.SelectedDate = null;


            GenderComboBox.SelectedItem = null;

            AddressTextBox.Text = "";
            PhoneNumberTextBox.Text = "";


            DepartmentComboBox.SelectedValue = null;
            PositionComboBox.SelectedValue = null;
            RoleComboBox.SelectedValue = null;
            CreateDatePicker.SelectedDate = null;
            SalaryTextBox.Text = "";
        }

        private void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            if (EmployeeDataGrid.SelectedItem is Employee selectedEmployee)
            {
                selectedEmployee.FullName = FullNameTextBox.Text;
                selectedEmployee.DateOfBirth = DateOfBirthDatePicker.SelectedDate.GetValueOrDefault();
                selectedEmployee.Gender = (GenderComboBox.SelectedItem as ComboBoxItem)?.Content.ToString() ?? throw new InvalidOperationException();
                selectedEmployee.Address = AddressTextBox.Text;
                selectedEmployee.PhoneNumber = PhoneNumberTextBox.Text;
                selectedEmployee.DepartmentId = (int)DepartmentComboBox.SelectedValue;
                selectedEmployee.PositionId = (int)PositionComboBox.SelectedValue;
                selectedEmployee.StartDate = CreateDatePicker.SelectedDate.GetValueOrDefault();
                selectedEmployee.ProfilePicture = ProfilePictureUrlTextBlock.Text;

                try
                {
                    // Validate Full Name
                    if (string.IsNullOrWhiteSpace(selectedEmployee.FullName))
                    {
                        MessageBox.Show("Tên không được để trống", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // Validate Phone Number
                    if (!Regex.IsMatch(selectedEmployee.PhoneNumber, @"^\d{10}$"))
                    {
                        MessageBox.Show("Số điện thoại không hợp lệ. Vui lòng nhập 10 chữ số.", "Lỗi",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // Validate Salary
                    if (string.IsNullOrWhiteSpace(SalaryTextBox.Text) || !double.TryParse(SalaryTextBox.Text, out double parsedSalary) || parsedSalary <= 0)
                    {
                        MessageBox.Show("Lương phải là một số hợp lệ lớn hơn 0", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    selectedEmployee.Salary = parsedSalary;

                    // Validate Date of Birth
                    if (selectedEmployee.DateOfBirth >= DateTime.Today)
                    {
                        MessageBox.Show("Lỗi ngày sinh lớn hơn ngày hiện tại", "Lỗi", MessageBoxButton.OK,
                            MessageBoxImage.Error);
                        return;
                    }

                    if (DateTime.Today.Year - selectedEmployee.DateOfBirth.Year < 18)
                    {
                        MessageBox.Show("Nhân viên phải trên 18 tuổi", "Lỗi", MessageBoxButton.OK,
                            MessageBoxImage.Error);
                        return;
                    }

                    // Validate Start Date
                    if (selectedEmployee.StartDate > DateTime.Today)
                    {
                        MessageBox.Show("Ngày tham gia không được lớn hơn ngày hiện tại", "Lỗi", MessageBoxButton.OK,
                            MessageBoxImage.Error);
                        return;
                    }
                    var salary = _salaryRepository.GetSalaryByEmployeeId(selectedEmployee.EmployeeId);
                    salary.BaseSalary = selectedEmployee.Salary;
                    _salaryRepository.UpdateSalary(salary);
                    selectedEmployee.ProfilePicture = ProfilePictureUrlTextBlock.Text;
                    _employeeRepository.UpdateEmployee(selectedEmployee);
                    LoadEmployees();

                    MessageBox.Show("Thông tin nhân viên đã được lưu thành công!", "Thông báo", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Có lỗi xảy ra khi lưu thông tin: {ex.Message}", "Lỗi", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một nhân viên để lưu thông tin!", "Cảnh báo", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = SearchTextBox.Text.ToLower();
            var filteredEmployees = _employeeRepository.GetAllEmployees()
                .Where(e => e.FullName.ToLower().Contains(searchText)).ToList();
            EmployeeDataGrid.ItemsSource = filteredEmployees;
        }
    }
}
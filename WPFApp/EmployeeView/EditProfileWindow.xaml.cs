using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Media.Imaging;
using BusinessObjects;
using DataAccessObjects;
using Repositories;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace WPFApp
{
    public partial class EditProfileWindow : Window
    {
        private readonly EmployeeRepository _employeeRepository;
        private Employee _currentEmployee;

        public EditProfileWindow(Employee currentEmployee)
        {
            InitializeComponent();
            FuhrmContext context = new FuhrmContext();
            EmployeeDAO employeeDAO = new EmployeeDAO(context);
            _employeeRepository = new EmployeeRepository(employeeDAO);

            _currentEmployee = currentEmployee;
            LoadEmployeeData();
        }

        private void LoadEmployeeData()
        {
            if (_currentEmployee != null)
            {
                FullNameTextBox.Text = _currentEmployee.FullName;
                DateOfBirthPicker.SelectedDate = _currentEmployee.DateOfBirth;
                GenderComboBox.Text = _currentEmployee.Gender;
                AddressTextBox.Text = _currentEmployee.Address;
                PhoneTextBox.Text = _currentEmployee.PhoneNumber;

                // Hiển thị avatar
                if (!string.IsNullOrEmpty(_currentEmployee.ProfilePicture))
                {
                    ProfileImage.Source = new BitmapImage(new Uri(_currentEmployee.ProfilePicture, UriKind.RelativeOrAbsolute));
                }
                else
                {
                    ProfileImage.Source = null; // Hiển thị ảnh mặc định nếu không có ảnh
                }
            }
            else
            {
                MessageBox.Show("No employee data available.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }

        private void ChooseImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                ProfileImage.Source = new BitmapImage(new Uri(filePath, UriKind.Absolute));
                _currentEmployee.ProfilePicture = filePath; // Cập nhật đường dẫn ảnh vào đối tượng
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentEmployee != null)
            {
                // Kiểm tra dữ liệu đầu vào trước khi lưu
                try
                {
                    // Validate Full Name
                    if (string.IsNullOrWhiteSpace(FullNameTextBox.Text))
                    {
                        MessageBox.Show("Tên không được để trống", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    if (Regex.IsMatch(FullNameTextBox.Text, @"\d"))
                    {
                        MessageBox.Show("Tên không được chứa chữ số", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // Validate Phone Number
                    if (!Regex.IsMatch(PhoneTextBox.Text, @"^\d{10}$"))
                    {
                        MessageBox.Show("Số điện thoại không hợp lệ. Vui lòng nhập 10 chữ số.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // Validate Date of Birth
                    if (DateOfBirthPicker.SelectedDate >= DateTime.Today)
                    {
                        MessageBox.Show("Ngày sinh không hợp lệ. Vui lòng chọn ngày sinh nhỏ hơn ngày hiện tại.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    if (DateTime.Today.Year - DateOfBirthPicker.SelectedDate.Value.Year < 18)
                    {
                        MessageBox.Show("Nhân viên phải trên 18 tuổi", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // Cập nhật thông tin từ giao diện vào đối tượng _currentEmployee
                    _currentEmployee.FullName = FullNameTextBox.Text;
                    _currentEmployee.DateOfBirth = DateOfBirthPicker.SelectedDate ?? _currentEmployee.DateOfBirth;
                    _currentEmployee.Gender = GenderComboBox.Text;
                    _currentEmployee.Address = AddressTextBox.Text;
                    _currentEmployee.PhoneNumber = PhoneTextBox.Text;

                    // Lưu thay đổi vào cơ sở dữ liệu
                    _employeeRepository.UpdateEmployee(_currentEmployee);
                    MessageBox.Show("Profile updated successfully!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Quay lại trang HomeEmployee và tải dữ liệu mới
                    HomeEmployee homeEmployeeWindow = new HomeEmployee();
                    homeEmployeeWindow.Show();
                    this.Close(); // Đóng cửa sổ EditProfileWindow hiện tại
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Có lỗi xảy ra khi lưu thông tin: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("No employee selected for update.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Trở về trang HomeEmployee
            HomeEmployee homeEmployeeWindow = new HomeEmployee();
            homeEmployeeWindow.Show();
            this.Close(); // Đóng cửa sổ EditProfileWindow hiện tại
        }

    }
}

﻿using Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BusinessObjects;

namespace WPFApp
{
    public partial class DepartmentManagement : Window
    {
        private readonly DepartmentRepository _departmentRepository;

        public DepartmentManagement()
        {
            InitializeComponent();
            _departmentRepository = new DepartmentRepository();
            CreateDatePicker.SelectedDate = DateTime.Now.Date; // Set today's date
            CreateDatePicker.IsEnabled = false; // Disable date picker to prevent changes
            LoadDepartments();
        }


        private void LoadDepartments()
        {
            var departments = _departmentRepository.GetDepartments();
            DepartmentDataGrid.ItemsSource = departments;
        }

        private void DepartmentDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DepartmentDataGrid.SelectedItem is Department selectedDepartment)
            {
                DepartmentIdTextBox.Text = selectedDepartment.DepartmentId.ToString();
                DepartmentNameTextBox.Text = selectedDepartment.DepartmentName;
                CreateDatePicker.SelectedDate = selectedDepartment.CreateDate;
                NumberOfEmployeeTextBox.Text = selectedDepartment.EmployeeCount.ToString();
            }
            else
            {
                Clear();
            }
        }


        private void Clear()
        {
            DepartmentIdTextBox.Text = string.Empty;
            DepartmentNameTextBox.Text = string.Empty;
            CreateDatePicker.SelectedDate = null;
            NumberOfEmployeeTextBox.Text = string.Empty;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var departmentName = DepartmentNameTextBox.Text;

            // Check if the department name already exists
            if (_departmentRepository.DoesDepartmentExist(departmentName))
            {
                MessageBox.Show("Tên phòng ban đã tồn tại. Vui lòng chọn tên khác.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var department = new Department
            {
                DepartmentName = departmentName,
                CreateDate = DateTime.Now.Date // Automatically set to today
            };

            _departmentRepository.AddDepartment(department);
            LoadDepartments();
        }


        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (DepartmentDataGrid.SelectedItem is Department selectedDepartment)
            {
                var newDepartmentName = DepartmentNameTextBox.Text;

                // Check if the new department name already exists (except for the current department)
                if (_departmentRepository.DoesDepartmentExist(newDepartmentName) && selectedDepartment.DepartmentName != newDepartmentName)
                {
                    MessageBox.Show("Tên phòng ban đã tồn tại. Vui lòng chọn tên khác.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                selectedDepartment.DepartmentName = newDepartmentName;
                selectedDepartment.CreateDate = CreateDatePicker.SelectedDate;

                _departmentRepository.UpdateDepartment(selectedDepartment);
                LoadDepartments();
            }
        }


        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (DepartmentDataGrid.SelectedItem is Department selectedDepartment)
            {
                _departmentRepository.RemoveDepartment(selectedDepartment);
                LoadDepartments();
            }
        }


        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = SearchTextBox.Text.ToLower();
            var filteredDepartments = _departmentRepository.GetDepartments()
                .Where(e => e.DepartmentName.ToLower().Contains(searchText)).ToList();
            DepartmentDataGrid.ItemsSource = filteredDepartments;
        }
    }
}

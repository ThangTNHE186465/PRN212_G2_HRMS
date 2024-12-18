﻿using BusinessObjects;
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
    /// <summary>
    /// Interaction logic for AccountManagement.xaml
    /// </summary>
    public partial class AccountManagement : Window
    {
        AccountRepository accountRepository;
        EmployeeRepository employeeRepository;
        SalaryRepository salaryRepository;

        public AccountManagement()
        {
            InitializeComponent();
            accountRepository = new AccountRepository();
            employeeRepository = new EmployeeRepository();
            salaryRepository = new SalaryRepository();
            LoadAccounts();
            LoadRoles();
        }

        private void LoadRoles()
        {
            try
            {
                var roles = accountRepository.GetRoles();
                RoleCombobox.ItemsSource = roles;
                RoleCombobox.DisplayMemberPath = "RoleName";
                RoleCombobox.SelectedValuePath = "RoleId";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading roles: {ex.Message}");
            }
            finally
            {
                ResetForm();
            }
        }

        public void LoadAccounts()
        {
            try
            {
                var accounts = accountRepository.GetAccounts();
                AccountDataGrid.ItemsSource = accounts;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading roles: {ex.Message}");
            }
            finally
            {
                ResetForm();
            }
        }

        private void AccountDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AccountDataGrid.SelectedItem is Account selectedAccount)
            {
                Account a = accountRepository.GetAccountById(selectedAccount.AccountId);
                DisplayAccountDetails(a);
            }
        }

        private void DisplayAccountDetails(Account account)
        {
            AccountIdTextBox.Text = account.AccountId.ToString();
            UsernameTextBox.Text = account.Username;
            PasswordTextBox.Text = account.Password;
            RoleCombobox.SelectedValue = account.Role.RoleId;
        }

        private void ResetForm()
        {
            AccountIdTextBox.Text = "";
            UsernameTextBox.Text = "";
            PasswordTextBox.Text = "";
            RoleCombobox.SelectedValue = 0;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ResetForm();
        }

        private void AddAccount_Click(object sender, RoutedEventArgs e)
        {
            if (UsernameTextBox.Text == "" || PasswordTextBox.Text == "" || RoleCombobox.SelectedValue == null)
            {
                MessageBox.Show("Please fill in all fields");
            }
            else
            {
                if (accountRepository.GetAccountByUserName(UsernameTextBox.Text) != null)
                {
                    MessageBox.Show("Username already exists");
                    return;
                }
                else
                {
                    Account account = new Account
                    {
                        Username = UsernameTextBox.Text,
                        Password = PasswordTextBox.Text,
                        RoleId = (int)RoleCombobox.SelectedValue
                    };
                    accountRepository.AddAccount(account);
                    if (account.RoleId == 2)
                    {

                        Employee employee = new Employee
                        {
                            FullName = "Default Name", 
                            DateOfBirth = new DateTime(2000, 1, 1),
                            Gender = "Not Specified", 
                            Address = "Default Address", 
                            PhoneNumber = "0000000000", 
                            DepartmentId = 1, 
                            PositionId = 1, 
                            Salary = 0,
                            StartDate = DateTime.Now, 
                            AccountId = account.AccountId 
                        };
                        employeeRepository.AddEmployee(employee);

                        var salary = new Salary
                        {
                            EmployeeId = employee.EmployeeId,
                            BaseSalary = 0,
                            Allowance = 0,
                            Bonus = 0,
                            Penalty = 0
                        };
                        salaryRepository.AddSalary(salary);
                    }
                    LoadAccounts();
                }
            }
        }


        private void EditAccount_Click(object sender, RoutedEventArgs e)
        {
            if (UsernameTextBox.Text == "" || PasswordTextBox.Text == "" || RoleCombobox.SelectedValue == null)
            {
                MessageBox.Show("Please fill in all fields");
            }
            else
            {
                var user = accountRepository.GetAccountByUserName(UsernameTextBox.Text);
                int accountId = int.Parse(AccountIdTextBox.Text);
                if (user != null && user.AccountId != accountId)
                {
                    MessageBox.Show("Username already exists");
                    return;
                }
                else
                {
                    Account account = new Account
                    {
                        AccountId = accountId,
                        Username = UsernameTextBox.Text,
                        Password = PasswordTextBox.Text,
                        RoleId = (int)RoleCombobox.SelectedValue
                    };
                    accountRepository.UpdateAccount(account);
                    MessageBox.Show("Account updated successfully.");
                    LoadAccounts();
                }
            }
        }

        private void DeleteAccount_Click(object sender, RoutedEventArgs e)
        {
            if (AccountIdTextBox.Text == "")
            {
                MessageBox.Show("Cannot delete without account id");
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Are you sure to delete?", "Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    int accountId = int.Parse(AccountIdTextBox.Text);
                    var employee = employeeRepository.GetEmployeeById(accountId);
                    if (employee != null)
                    {
                        employeeRepository.DeleteEmployee(employee.EmployeeId);
                    }
                    accountRepository.DeleteAccount(accountId);
                    LoadAccounts();
                }
            }
        }
    }
}

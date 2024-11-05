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
                var em = employeeRepository.GetEmployeeByAccountId(currentAccount.AccountId);
                WelcomeTextBlock.Text = $"Xin chào {em.FullName}";
                NameTextBox.Text = em.FullName;
                BirthTextBox.Text = em.DateOfBirth.ToString(CultureInfo.CurrentCulture).Split(" ")[0];
                GenderTextBox.Text = em.Gender;
                AddressTextBox.Text = em.Address;
                PhoneTextBox.Text = em.PhoneNumber;
                StartTextBox.Text = em.StartDate.ToString(CultureInfo.InvariantCulture).Split(" ")[0];
                DepartmentTextBox.Text = em.Department.DepartmentName;
                PositionTextBox.Text = em.Position.PositionName;
                SalaryTextBox.Text = em.Salary.ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                WelcomeTextBlock.Text = "Xin chào nhân viên";
            }
        }
    }
}

using System.Windows;
using BusinessObjects;
using Repositories;
using System.Windows.Input;

namespace WPFApp
{
    public partial class LoginScreen : Window
    {
        private readonly AccountRepository _accountRepository;
        public LoginScreen()
        {
            InitializeComponent();
            _accountRepository = new AccountRepository();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (txtUsername != null && txtPassword != null)
            {
                var account = _accountRepository.GetAccountByUserName(txtUsername.Text);
                if (account != null)
                {
                    if (account.Password.Equals(txtPassword.Password))
                    {
                        SessionManager.CurrentAccount = account;
                        var employee = _accountRepository.GetEmployeeByUsername(account.AccountId);
                        if (account.Role.RoleName.Equals("Admin"))
                        {
                            AdminDashboard adminDashboard = new AdminDashboard();
                            adminDashboard.Show();
                            this.Close();
                        }
                        else if (account.Role.RoleName.Equals("Employee"))
                        {
                            HomeEmployee homeEmployee = new HomeEmployee();
                            homeEmployee.Show();
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Incorrect password");
                    }
                }
                else
                {
                    MessageBox.Show("Incorrect username");
                }
            }
            else
            {
                MessageBox.Show("Please enter username and password");
            }
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Trigger the login button click
                btnLogin_Click(this, new RoutedEventArgs());
            }
        }
    }

    public static class SessionManager
    {
        public static Account? CurrentAccount { get; set; }
    }
}

using System.Windows;
using HRM.ViewModels;

namespace HRM.Views;

public partial class LoginView : Window
{
    public LoginView()
    {
        InitializeComponent();
        PasswordBox.PasswordChanged += (s, e) =>
        {
            if (DataContext is LoginViewModel vm)
            {
                vm.Password = PasswordBox.Password;
            }
        };
    }
}
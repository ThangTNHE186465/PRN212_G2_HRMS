using System.Windows;
using HRM.ViewModels;

namespace HRM.Views;

public partial class LoginView : Window
{
    public LoginView()
    {
        InitializeComponent();
        DataContext = new LoginViewModel(this);
        PasswordBox.PasswordChanged += (s, e) =>
        {
            if (DataContext is LoginViewModel vm)
            {
                vm.Password = PasswordBox.Password;
            }
        };
    }
}
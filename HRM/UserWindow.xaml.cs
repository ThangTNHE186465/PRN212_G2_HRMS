using System.Windows;
using HRM.Views.Admin;
using HRM.Views.User;

namespace HRM;

public partial class UserWindow : Window
{
    public UserWindow()
    {
        InitializeComponent();
        UserFrame.Navigate(new UserDashboard());
    }
}
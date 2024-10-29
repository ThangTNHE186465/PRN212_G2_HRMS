using System.Windows;
using HRM.Views.Admin;
using AdminViewModel = HRM.ViewModels.Admin.AdminViewModel;

namespace HRM;

public partial class AdminWindow : Window
{
    public AdminWindow()
    {
        InitializeComponent();
        AdminFrame.Navigate(new AdminDashboard());
        DataContext = new AdminViewModel(AdminFrame);
    }
}
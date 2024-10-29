// using System.Collections.ObjectModel;
// using System.Windows;
// using System.Windows.Input;
// using CommunityToolkit.Mvvm.Input;
// using HRM.Models;
// using HRM.Service;
//
// namespace HRM.ViewModels;
//
// public class EmployeeViewModel : BaseViewModel
// {
//     // private readonly IEmployeeService _employeeService;
//     private readonly INavigationService _navigationService;
//     private ObservableCollection<Employee> _employees;
//     private Employee _selectedEmployee;
//     private string _searchText;
//     private bool _isBusy;
//
//     public EmployeeViewModel(
//         // IEmployeeService employeeService,
//         INavigationService navigationService)
//     {
//         // _employeeService = employeeService;
//         _navigationService = navigationService;
//         
//         // Initialize commands
//         AddEmployeeCommand = new RelayCommand(ExecuteAddEmployee);
//         EditEmployeeCommand = new RelayCommand(ExecuteEditEmployee, CanExecuteEditEmployee);
//         DeleteEmployeeCommand = new RelayCommand(ExecuteDeleteEmployee, CanExecuteDeleteEmployee);
//         SearchCommand = new RelayCommand<string>(ExecuteSearch);
//         RefreshCommand = new RelayCommand(ExecuteRefresh);
//     }
//
//     // Properties
//     public ObservableCollection<Employee> Employees
//     {
//         get => _employees;
//         set => SetProperty(ref _employees, value);
//     }
//
//     public Employee SelectedEmployee
//     {
//         get => _selectedEmployee;
//         set
//         {
//             if (SetProperty(ref _selectedEmployee, value))
//             {
//                 // Khi selected employee thay đổi, cập nhật trạng thái của các commands
//                 (EditEmployeeCommand as RelayCommand)?.NotifyCanExecuteChanged();
//                 (DeleteEmployeeCommand as RelayCommand)?.NotifyCanExecuteChanged();
//             }
//         }
//     }
//
//     public string SearchText
//     {
//         get => _searchText;
//         set
//         {
//             if (SetProperty(ref _searchText, value))
//             {
//                 // Tự động search khi text thay đổi
//                 SearchCommand.Execute(value);
//             }
//         }
//     }
//
//     public bool IsBusy
//     {
//         get => _isBusy;
//         set => SetProperty(ref _isBusy, value);
//     }
//
//     // Commands
//     public ICommand AddEmployeeCommand { get; }
//     public ICommand EditEmployeeCommand { get; }
//     public ICommand DeleteEmployeeCommand { get; }
//     public ICommand<string> SearchCommand { get; }
//     public ICommand RefreshCommand { get; }
//
//     // Command Implementations
//     private async void ExecuteAddEmployee()
//     {
//         _navigationService.NavigateTo("EmployeeDetailView");
//     }
//
//     private void ExecuteEditEmployee()
//     {
//         if (SelectedEmployee != null)
//         {
//             _navigationService.NavigateTo("EmployeeDetailView", SelectedEmployee);
//         }
//     }
//
//     private async void ExecuteDeleteEmployee()
//     {
//         if (SelectedEmployee == null) return;
//
//         var result = MessageBox.Show(
//             $"Bạn có chắc muốn xóa nhân viên {SelectedEmployee.Name}?",
//             "Xác nhận xóa",
//             MessageBoxButton.YesNo,
//             MessageBoxImage.Question);
//
//         if (result == MessageBoxResult.Yes)
//         {
//             try
//             {
//                 IsBusy = true;
//                 await _employeeService.DeleteEmployeeAsync(SelectedEmployee.Id);
//                 Employees.Remove(SelectedEmployee);
//             }
//             catch (Exception ex)
//             {
//                 MessageBox.Show(
//                     "Không thể xóa nhân viên. " + ex.Message,
//                     "Lỗi",
//                     MessageBoxButton.OK,
//                     MessageBoxImage.Error);
//             }
//             finally
//             {
//                 IsBusy = false;
//             }
//         }
//     }
//
//     private async void ExecuteSearch(string searchText)
//     {
//         try
//         {
//             IsBusy = true;
//             var employees = await _employeeService.SearchEmployeesAsync(searchText);
//             Employees = new ObservableCollection<Employee>(employees);
//         }
//         finally
//         {
//             IsBusy = false;
//         }
//     }
//
//     private async void ExecuteRefresh()
//     {
//         await LoadEmployeesAsync();
//     }
//
//     // Helper Methods
//     private bool CanExecuteEditEmployee() => SelectedEmployee != null;
//     private bool CanExecuteDeleteEmployee() => SelectedEmployee != null;
//
//     public async Task LoadEmployeesAsync()
//     {
//         try
//         {
//             IsBusy = true;
//             var employees = await _employeeService.GetAllEmployeesAsync();
//             Employees = new ObservableCollection<Employee>(employees);
//         }
//         finally
//         {
//             IsBusy = false;
//         }
//     }
// }
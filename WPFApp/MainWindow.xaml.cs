using System.Windows;
using BusinessObjects;
using Repositories;
using WPFApp.EmployeeView;

namespace WPFApp
{
    public partial class MainWindow : Window
    {
        private readonly int employeeID;
        public LeaveRequest LoggedInEmployee { get; set; }
        public MainWindow(int employeeId)
        {
            InitializeComponent();
            employeeID = employeeId;
            LoadLeavesRequestByID();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AttendanceForm attendanceForm = new AttendanceForm(employeeID);
            attendanceForm.Show();
            this.Close();
        }
        public void LoadLeavesRequestByID()
        {
            LeaveRequestRepository leaveRepository = new LeaveRequestRepository();

            // Sử dụng trực tiếp employeeID để lấy danh sách yêu cầu nghỉ phép
            var leaveRequests = leaveRepository.GetLeaveRequestsByEmployeeID(employeeID);

            // Gán dữ liệu cho ViewLeaveRequest
            ViewLeaveRequest.ItemsSource = leaveRequests;


        }
    }
}
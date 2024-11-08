using System.Windows;
using System.Windows.Controls;
using BusinessObjects;
using DataAccessObjects;
using Repositories;

namespace WPFApp
{
    /// <summary>
    /// Interaction logic for LeaveRequestView.xaml
    /// </summary>
    public partial class LeaveRequestView : Window
    {
        private readonly LeaveRequestRepository _leaveRepository;
        public LeaveRequestView()
        {
            InitializeComponent();
            _leaveRepository = new LeaveRequestRepository();
            LoadLeaveRequest();
        }
        public void LoadLeaveRequest()
        {
            var leaveRequest = _leaveRepository.getAllLeaveRequest();
            LeaveRequestDataGrid.ItemsSource = leaveRequest;
        }
        public void LeaveRequestDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LeaveRequestDataGrid.SelectedItem is LeaveRequest selectedLeave)
            {
                int leaveRequestID = selectedLeave.LeaveRequestId;
                LeaveRequest leaveRequestDetail = _leaveRepository.getLeaveRequest(leaveRequestID);
                if (leaveRequestDetail != null)
                {
                    EmployeeNameText.Text = leaveRequestDetail.Employee.FullName;
                    DepartmentNameText.Text = leaveRequestDetail.Employee.Department.DepartmentName;
                    LeaveTypeText.Text = leaveRequestDetail.LeaveType.ToString();
                    StartDateText.Text = leaveRequestDetail.StartDate.ToString();
                    EndDateText.Text = leaveRequestDetail.EndDate.ToString();
                    StatusText.Text = leaveRequestDetail.Status.ToString();

                }
            }
        }
        private void ChangeStatus_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (LeaveRequestDataGrid.SelectedItem is LeaveRequest selectedLeave)
            {
                int leaveRequestID = selectedLeave.LeaveRequestId;
                string newStatus = button.Content.ToString(); // Lấy nội dung của nút (Approved hoặc Rejected)
                int leaveRequestId = leaveRequestID;
                var dao = new LeaveRequestDAO();

                try
                {
                    dao.ChangeStatus(leaveRequestId, newStatus);
                    MessageBox.Show($"Status changed to {newStatus}");
                    LoadLeaveRequest();
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}

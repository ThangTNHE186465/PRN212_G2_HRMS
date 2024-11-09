using System.Windows;
using BusinessObjects;
using Repositories;

namespace WPFApp
{
    public partial class AttendanceForm : Window
    {
        private readonly int _employeeID;
        public AttendanceForm(int employeeID)
        {
            InitializeComponent();
            _employeeID = employeeID;
        }

        private void btnSubmitLeaveRequest_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var leaveRequestRepo = new LeaveRequestRepository();

                if (_employeeID != null)
                {
                    if (StartDate.SelectedDate.HasValue && EndDate.SelectedDate.HasValue)
                    {
                        var today = DateOnly.FromDateTime(DateTime.Today);
                        var selectedStartDate = DateOnly.FromDateTime(StartDate.SelectedDate.Value);
                        var selectedEndDate = DateOnly.FromDateTime(EndDate.SelectedDate.Value);

                        if (selectedStartDate >= today)
                        {
                            if (selectedEndDate > selectedStartDate)
                            {
                                int requestedDays = (selectedEndDate.ToDateTime(TimeOnly.MinValue) - selectedStartDate.ToDateTime(TimeOnly.MinValue)).Days + 1;

                                int currentMonth = selectedStartDate.Month;
                                int currentYear = selectedStartDate.Year;
                                int totalLeaveDaysInMonth = leaveRequestRepo.GetTotalLeaveDaysInMonth(_employeeID, currentYear, currentMonth);

                                if (totalLeaveDaysInMonth + requestedDays <= 4)
                                {
                                    var leaveRequest = new LeaveRequest
                                    {
                                        EmployeeId = _employeeID,
                                        LeaveType = LeaveType.Text,
                                        StartDate = selectedStartDate,
                                        EndDate = selectedEndDate,
                                        Status = "Pending"
                                    };

                                    leaveRequestRepo.AddLeaveRequest(leaveRequest);

                                    MessageBox.Show("Yêu cầu nghỉ phép đã được gửi thành công!");
                                    MainWindow mainWindow = new MainWindow(_employeeID);
                                    mainWindow.Show();
                                    this.Close(); 
                                }
                                else
                                {
                                    MessageBox.Show("Số ngày nghỉ trong tháng không được vượt quá 4 ngày. ");
                                    return; 
                                }
                            }
                            else
                            {
                                MessageBox.Show("Ngày kết thúc phải lớn hơn ngày bắt đầu.");
                                return; 
                            }
                        }
                        else
                        {
                            MessageBox.Show("Ngày bắt đầu phải lớn hơn hoặc bằng ngày hiện tại.");
                            return; 
                        }
                    }
                    else
                    {
                        MessageBox.Show("Vui lòng chọn ngày bắt đầu và ngày kết thúc.");
                        return; 
                    }
                }
                else
                {
                    MessageBox.Show("Không tìm thấy nhân viên cho tài khoản hiện tại.");
                    return; 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}");
            }
        }




    }
}
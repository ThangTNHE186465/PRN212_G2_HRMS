using BusinessObjects;
using DataAccessObjects;

public class AttendanceRepository : IAttendanceRepository
{
    public void AddAttendance(Attendance attendance) => AttendanceDAO.AddAttendance(attendance);
    public List<Attendance>? SearchAttendance(string employeeName) => AttendanceDAO.SearchAttendance(employeeName);
    public void RemoveAttendance(Attendance attendance) => AttendanceDAO.RemoveAttendance(attendance);
    public void UpdateAttendance(Attendance attendance) => AttendanceDAO.UpdateAttendance(attendance);
    public List<Attendance> GetAttendances() => AttendanceDAO.GetAttendances();
    public void UpdateAttendanceStatus(int employeeId, string status) => AttendanceDAO.UpdateAttendanceStatus(employeeId, status);
    public void CheckoutAttendance(int employeeId) => AttendanceDAO.CheckoutAttendance(employeeId); // Thêm phương thức mới
}

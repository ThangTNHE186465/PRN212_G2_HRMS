using BusinessObjects;

public interface IAttendanceRepository
{
    List<Attendance> GetAttendances();
    void AddAttendance(Attendance attendance);
    List<Attendance>? SearchAttendance(string employeeName);
    void RemoveAttendance(Attendance attendance);
    void UpdateAttendance(Attendance attendance);
    void UpdateAttendanceStatus(int employeeId, string status);
    void CheckoutAttendance(int employeeId); 
}

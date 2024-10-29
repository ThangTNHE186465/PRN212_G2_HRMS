using HRM.Models;

namespace HRM.Service;

public interface IAttendanceService
{
    // Task RecordAttendanceAsync(int employeeId, AttendanceType type);
    Task<IEnumerable<Attendance>> GetAttendanceReportAsync(int employeeId, DateTime startDate, DateTime endDate);
    // Task<MonthlyAttendanceReport> GenerateMonthlyReportAsync(int employeeId, int month, int year);
}
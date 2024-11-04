using BusinessObjects;
using DataAccessObjects;
using System.Collections.Generic;

namespace Repositories
{
    public class AttendanceRepository : IAttendanceRepository
    {
        public void AddAttendance(Attendance attendance)
            => AttendanceDAO.AddAttendance(attendance);

        public List<Attendance>? SearchAttendance(string employeeName)
            => AttendanceDAO.SearchAttendance(employeeName);

        public void RemoveAttendance(Attendance attendance)
            => AttendanceDAO.RemoveAttendance(attendance);

        public void UpdateAttendance(Attendance attendance)
            => AttendanceDAO.UpdateAttendance(attendance);

        public List<Attendance> GetAttendances()
            => AttendanceDAO.GetAttendances();
    }

    public class AttendanceRepository : IAttendanceRepository
    {
        public void AddAttendance(Attendance attendance)
            => AttendanceDAO.AddAttendance(attendance);

        public List<Attendance>? SearchAttendance(string employeeName)
            => AttendanceDAO.SearchAttendance(employeeName);

        public void RemoveAttendance(Attendance attendance)
            => AttendanceDAO.RemoveAttendance(attendance);

        public void UpdateAttendance(Attendance attendance)
            => AttendanceDAO.UpdateAttendance(attendance);

        public List<Attendance> GetAttendances()
            => AttendanceDAO.GetAttendances();
    }
}
}

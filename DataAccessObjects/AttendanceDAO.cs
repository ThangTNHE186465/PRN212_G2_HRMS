using BusinessObjects;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DataAccessObjects
{
    public static class AttendanceDAO
    {
        public static List<Attendance> GetAttendances()
        {
            using (var context = new FuhrmContext())
            {
                return context.Attendances.Include(a => a.Employee).ToList();
            }
        }

        public static void AddAttendance(Attendance attendance)
        {
            using (var context = new FuhrmContext())
            {
                context.Attendances.Add(attendance);
                context.SaveChanges();
            }
        }

        public static List<Attendance> SearchAttendance(string employeeName)
        {
            using (var context = new FuhrmContext())
            {
                return context.Attendances
                    .Include(a => a.Employee)
                    .Where(a => a.Employee.FullName.Contains(employeeName))
                    .ToList();
            }
        }

        public static void RemoveAttendance(Attendance attendance)
        {
            using (var context = new FuhrmContext())
            {
                context.Attendances.Remove(attendance);
                context.SaveChanges();
            }
        }

        public static void UpdateAttendance(Attendance attendance)
        {
            using (var context = new FuhrmContext())
            {
                context.Attendances.Update(attendance);
                context.SaveChanges();
            }
        }

        public static void UpdateAttendanceStatus(int employeeId, string status)
        {
            using (var context = new FuhrmContext())
            {
                var attendance = context.Attendances
                                        .FirstOrDefault(a => a.EmployeeId == employeeId && a.Date == DateOnly.FromDateTime(DateTime.Now));
                if (attendance != null)
                {
                    attendance.Status = status;
                    context.SaveChanges();
                }
            }
        }
        public static void CheckoutAttendance(int employeeId)
        {
            using (var context = new FuhrmContext())
            {
                var attendance = context.Attendances
                                        .FirstOrDefault(a => a.EmployeeId == employeeId && a.Date == DateOnly.FromDateTime(DateTime.Now) && a.Status == "Present");
                if (attendance != null)
                {
                    attendance.Status = "Present"; 
                    context.SaveChanges();
                }
            }
        }


    }
}

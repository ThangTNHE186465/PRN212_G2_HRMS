using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects
{

    public class LeaveRequestDAO
    {
        public List<LeaveRequest> getAllLeaveRequest()
        {
            using var _context = new FuhrmContext();
            var leaveList = new List<LeaveRequest>();
            try
            {
                leaveList = _context.LeaveRequests.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return leaveList;
        }
        public List<LeaveRequest> GetLeaveRequestsByEmployeeID(int employeeId)
        {
            try
            {
                using var context = new FuhrmContext();
                return context.LeaveRequests
                    .Include(lr => lr.Employee)
                    .Where(lr => lr.EmployeeId == employeeId)
                    .OrderByDescending(lr => lr.StartDate)
                    .ToList();
            }
            catch (Exception ex)
            {
                // Log the exception if you have logging configured
                throw new Exception($"Error retrieving leave requests for employee {employeeId}: {ex.Message}", ex);
            }
        }
        public LeaveRequest getLeaveRequest(int id)
        {
            using var _context = new FuhrmContext();
            var leaveRequestDetail = _context.LeaveRequests
                    .Include(e => e.Employee)
                    .ThenInclude(d => d.Department)
                    .FirstOrDefault(l => l.LeaveRequestId == id);
            return leaveRequestDetail;
        }
        public void ChangeStatus(int leaveRequestId, string newStatus)
        {
            using var _context = new FuhrmContext();
            try
            {
                var leaveRequest = _context.LeaveRequests.FirstOrDefault(lr => lr.LeaveRequestId == leaveRequestId);
                if (leaveRequest == null)
                {
                    throw new Exception("Leave request not found.");
                }
                leaveRequest.Status = newStatus;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating status: " + ex.Message);
            }
        }
        public void AddLeaveRequest(LeaveRequest leaveRequest)
        {
            using var _context = new FuhrmContext();
            _context.LeaveRequests.Add(leaveRequest);
            _context.SaveChanges();
        }
        public Employee GetEmployeeByAccountId(int accountId)
        {
            using var _context = new FuhrmContext();
            return _context.Employees.FirstOrDefault(e => e.AccountId == accountId);
        }

    }
}

using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface ILeaveRequestRepository
    {
        List<LeaveRequest> getAllLeaveRequest();
        LeaveRequest getLeaveRequest(int id);
        void ChangeStatus(int leaveRequestId, string newStatus);
        void AddLeaveRequest(LeaveRequest leaveRequest);
        Employee GetEmployeeByAccountId(int accountId);
        List<LeaveRequest> GetLeaveRequestsByEmployeeID(int employeeId);
        int GetTotalLeaveDaysInMonth(int employeeId, int year, int month); // Thêm dòng này
    }

}

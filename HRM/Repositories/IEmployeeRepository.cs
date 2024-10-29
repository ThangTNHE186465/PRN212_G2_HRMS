using HRM.Models;

namespace HRM.Repositories;

public interface IEmployeeRepository
{
    Task<IEnumerable<Employee>> GetEmployeesByDepartmentAsync(int departmentId);
    Task<IEnumerable<Employee>> SearchEmployeesAsync(
        string searchTerm, 
        int? departmentId, 
        DateTime? startDate, 
        DateTime? endDate);
}
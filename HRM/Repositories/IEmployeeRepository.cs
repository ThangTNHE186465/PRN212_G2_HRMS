using HRM.Models;

namespace HRM.Repositories;

public interface IEmployeeRepository : IBaseRepository<Employee>
{
    Task<IEnumerable<Employee>> GetEmployeesByDepartmentAsync(int departmentId);
    Task<IEnumerable<Employee>> SearchEmployeesAsync(
        string? searchTerm,
        int? departmentId,
        DateTime? startDate,
        DateTime? endDate);
    Task<bool> IsExist(string employeeEmployeeCode);
    Task<IEnumerable<Employee>> GetActiveEmployeesAsync();
}
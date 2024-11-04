using BusinessObjects;
using System.Collections.Generic;

namespace Repositories
{
    public interface IEmployeeRepository
    {
        List<Employee> GetAllEmployees();
        Employee GetEmployeeById(int employeeId);

        void AddEmployee(Employee employee);
        void UpdateEmployee(Employee employee);
        void DeleteEmployee(int employeeId);
    }
}

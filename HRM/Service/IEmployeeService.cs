using System.IO;
using HRM.Models;
using HRM.Models.Enum;
using Microsoft.Extensions.FileProviders;

namespace HRM.Service.ServiceImpl;

public interface IEmployeeService
{
    Task<Employee> GetByIdAsync(int id);
    Task<Employee> GetByEmployeeCodeAsync(string code);
    Task<IEnumerable<Employee>> GetAllAsync();
    Task<Employee> CreateEmployeeAsync(Employee employee);
    Task<Employee> UpdateEmployeeAsync(int id, Employee employee);
    Task DeleteEmployeeAsync(int id);
    Task<string> UploadAvatarAsync(int id, IFileProvider avatar);
    Task<IEnumerable<Employee>> SearchEmployeesAsync(Employee searchCriteria);
    Task<bool> ValidateEmployeeCodeAsync(string code);
    Task<Employee> GetEmployeeStatisticsAsync();
    Task<byte[]> ExportToExcelAsync(Employee criteria);
    Task<bool> UpdateEmployeeStatusAsync(int id, EmployeeStatus status);
    Task<decimal> CalculateTotalSalaryAsync(int employeeId, int month, int year);
}
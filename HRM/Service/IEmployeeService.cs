using System.IO;
using HRM.Models;
using HRM.Models.Enum;
using Microsoft.Extensions.FileProviders;

namespace HRM.Service;

public interface IEmployeeService
{
    Task<IEnumerable<Employee?>> GetAllEmployees();

    Task<Employee?> GetByEmployeeCodeAsync(string code);

    Task<Employee?> GetByIdAsync(int id);

    Task<Employee?> CreateEmployeeAsync(Employee? employee);

    Task<Employee?> UpdateEmployeeAsync(int id, Employee? employee);

    Task DeleteEmployeeAsync(int id);

    Task<bool> UploadAvatarAsync(int id, string avatar);

    Task<IEnumerable<Employee>> SearchEmployeesAsync(string searchTerm,
        int? departmentId,
        DateTime? startDate,
        DateTime? endDate);

    //Task<EmployeeStatistics> GetEmployeeStatisticsAsync();

    Task<byte[]> ExportToExcelAsync(Employee criteria);

    Task<bool> UpdateEmployeeStatusAsync(int id, EmployeeStatus status);

    Task<IEnumerable<Employee?>> FilterEmployeesAsync(string selectedGender, string selectedSalaryRange,
        DateTime? startDate, DateTime? endDate);
}
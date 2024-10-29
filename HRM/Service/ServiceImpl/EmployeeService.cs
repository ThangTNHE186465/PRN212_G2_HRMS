using System.ComponentModel.DataAnnotations;
using System.IO;
using HRM.Models;
using HRM.Models.Enum;
using HRM.Repositories;
using HRM.Repositories.RepositoryImpl;
using Microsoft.Extensions.Logging;

namespace HRM.Service.ServiceImpl;

public class EmployeeService
{
    private readonly IBaseRepository<Employee> _employeeRepository;
    private readonly ILogger<EmployeeService> _logger;
    private readonly IActivityLogService _activityLogService;
    // private readonly ISalaryCalculationService _salaryCalculationService;

    public EmployeeService(
    )
    {
        _employeeRepository = new EmployeeRepository(new HrmContext());
        _logger = new Logger<EmployeeService>(new LoggerFactory());
        _activityLogService = new ActivityLogService();
        // _salaryCalculationService = salaryCalculationService;
    }

    public async Task<Employee> GetByIdAsync(int id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);
        if (employee == null)
        {
            throw new KeyNotFoundException($"Employee with ID {id} not found");
        }

        return employee;
    }

    public async Task<IEnumerable<Employee>> GetAllAsync()
    {
        return await _employeeRepository.GetAllAsync();
    }

    public async Task<Employee> CreateEmployeeAsync(Employee employee)
    {
        try
        {
            // // Business validation
            // if (string.IsNullOrEmpty(employee.EmployeeCode))
            // {
            //     throw new ValidationException("Employee code is required");
            // }
            //
            // if (!await _employeeRepository.IsEmployeeCodeUniqueAsync(employee.EmployeeCode))
            // {
            //     throw new ValidationException($"Employee code {employee.EmployeeCode} already exists");
            // }

            // Validate required fields
            ValidateEmployee(employee);

            // Set default values for new employee
            employee.Status = EmployeeStatus.Active;
            employee.HireDate = employee.HireDate == default ? DateOnly.FromDateTime(DateTime.Now) : employee.HireDate;

            // Save employee
            var createdEmployee = await _employeeRepository.AddAsync(employee);

            // Log activity
            await _activityLogService.LogActivityAsync(
                "Employee Created",
                $"Created employee: {employee.FirstName + " " + employee.LastName} ({employee.Id})"
            );

            return createdEmployee;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating employee: {EmployeeCode}", employee.EmployeeCode);
            throw;
        }
    }

    public async Task<Employee> UpdateEmployeeAsync(int id, Employee updatedEmployee)
    {
        var existingEmployee = await GetByIdAsync(id);

        try
        {
            // Validate required fields
            ValidateEmployee(updatedEmployee);

            // Preserve certain original values
            updatedEmployee.Id = existingEmployee.Id;
            updatedEmployee.ModifiedDate = DateTime.Now;
            updatedEmployee.PhotoPath = existingEmployee.PhotoPath;

            await _employeeRepository.UpdateAsync(updatedEmployee);

            await _activityLogService.LogActivityAsync(
                "Employee Updated",
                $"Updated employee: {updatedEmployee.FullName} ({updatedEmployee.EmployeeCode})"
            );

            return updatedEmployee;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating employee: {Id}", id);
            throw;
        }
    }

    // public async Task<string> UploadAvatarAsync(int id, string avatar)
    // {
    //     var employee = await GetByIdAsync(id);
    //
    //     try
    //     {
    //         // Delete existing avatar if present
    //         if (!string.IsNullOrEmpty(employee.PhotoPath))
    //         {
    //             
    //         }
    //         
    //
    //         // Save file
    //         var filePath = await _fileStorageService.SaveFileAsync(avatar, fileName);
    //
    //         // Update employee record
    //         employee.AvatarPath = filePath;
    //         await _employeeRepository.UpdateAsync(employee);
    //
    //         return filePath;
    //     }
    //     catch (Exception ex)
    //     {
    //         _logger.LogError(ex, "Error uploading avatar for employee: {Id}", id);
    //         throw;
    //     }
    // }
    //
    // public async Task<IEnumerable<Employee>> SearchEmployeesAsync(EmployeeSearchCriteria criteria)
    // {
    //     return await _employeeRepository.SearchAsync(criteria);
    // }
    //
    // public async Task<bool> UpdateEmployeeStatusAsync(int id, EmployeeStatus status)
    // {
    //     var employee = await GetByIdAsync(id);
    //
    //     employee.Status = status;
    //     await _employeeRepository.UpdateAsync(employee);
    //
    //     await _activityLogService.LogActivityAsync(
    //         "Employee Status Updated",
    //         $"Updated status for employee {employee.FullName} to {status}"
    //     );
    //
    //     return true;
    // }
    //
    // public async Task<decimal> CalculateTotalSalaryAsync(int employeeId, int month, int year)
    // {
    //     var employee = await GetByIdAsync(employeeId);
    //
    //     return await _salaryCalculationService.CalculateTotalSalaryAsync(
    //         employee,
    //         new DateTime(year, month, 1)
    //     );
    // }
    //
    // public async Task<byte[]> ExportToExcelAsync(EmployeeSearchCriteria criteria)
    // {
    //     try
    //     {
    //         var employees = await _employeeRepository.SearchAsync(criteria);
    //         using var package = new ExcelPackage();
    //         var worksheet = package.Workbook.Worksheets.Add("Employees");
    //
    //         // Add headers
    //         var headers = new string[]
    //         {
    //             "Employee Code",
    //             "Full Name",
    //             "Department",
    //             "Position",
    //             "Start Date",
    //             "Basic Salary",
    //             "Status"
    //         };
    //
    //         for (int i = 0; i < headers.Length; i++)
    //         {
    //             worksheet.Cells[1, i + 1].Value = headers[i];
    //             worksheet.Cells[1, i + 1].Style.Font.Bold = true;
    //         }
    //
    //         // Add data
    //         int row = 2;
    //         foreach (var employee in employees)
    //         {
    //             worksheet.Cells[row, 1].Value = employee.EmployeeCode;
    //             worksheet.Cells[row, 2].Value = employee.FullName;
    //             worksheet.Cells[row, 3].Value = employee.Department?.Name;
    //             worksheet.Cells[row, 4].Value = employee.Position?.Name;
    //             worksheet.Cells[row, 5].Value = employee.StartDate.ToShortDateString();
    //             worksheet.Cells[row, 6].Value = employee.BasicSalary;
    //             worksheet.Cells[row, 7].Value = employee.Status.ToString();
    //             row++;
    //         }
    //
    //         worksheet.Cells.AutoFitColumns();
    //
    //         return await package.GetAsByteArrayAsync();
    //     }
    //     catch (Exception ex)
    //     {
    //         _logger.LogError(ex, "Error exporting employees to Excel");
    //         throw;
    //     }
    // }
    //
    // public async Task DeleteEmployeeAsync(int id)
    // {
    //     var employee = await GetByIdAsync(id);
    //
    //     try
    //     {
    //         // Delete avatar if exists
    //         if (!string.IsNullOrEmpty(employee.AvatarPath))
    //         {
    //             await _fileStorageService.DeleteFileAsync(employee.AvatarPath);
    //         }
    //
    //         await _employeeRepository.DeleteAsync(id);
    //
    //         await _activityLogService.LogActivityAsync(
    //             "Employee Deleted",
    //             $"Deleted employee: {employee.FullName} ({employee.EmployeeCode})"
    //         );
    //     }
    //     catch (Exception ex)
    //     {
    //         _logger.LogError(ex, "Error deleting employee: {Id}", id);
    //         throw;
    //     }
    // }
    
    private void ValidateEmployee(Employee employee)
    {
        var validationErrors = new List<string>();
    
        if (string.IsNullOrEmpty(employee.FirstName))
            validationErrors.Add("First name is required");
    
        if (string.IsNullOrEmpty(employee.LastName))
            validationErrors.Add("Last name is required");
    
        if (employee.DateOfBirth == default)
            validationErrors.Add("Date of birth is required");
    
        if (employee.DateOfBirth < DateOnly.FromDateTime(DateTime.Now.AddYears(-18)))
            validationErrors.Add("Employee must be at least 18 years old");
    
        if (employee.DepartmentId <= 0)
            validationErrors.Add("Department is required");
    
        // if (employee.PositionId <= 0)
        //     validationErrors.Add("Position is required");
    
        if (employee.BasicSalary <= 0)
            validationErrors.Add("Basic salary must be greater than 0");
    
        if (validationErrors.Any())
        {
            throw new ValidationException(
                "Employee validation failed: " + string.Join(", ", validationErrors)
            );
        }
    }
    
    // public async Task<EmployeeStatistics> GetEmployeeStatisticsAsync()
    // {
    //     var statistics = new EmployeeStatistics
    //     {
    //         TotalEmployees = _employeeRepository.GetAllAsync().Result.Count(),
    //         ActiveEmployees = (await _employeeRepository.GetActiveEmployeesAsync()).Count(),
    //         DepartmentStatistics = new List<DepartmentStatistics>()
    //     };
    //
    //     var employees = await _employeeRepository.GetActiveEmployeesAsync();
    //     var departmentGroups = employees.GroupBy(e => e.Department?.Name);
    //
    //     foreach (var group in departmentGroups)
    //     {
    //         if (!string.IsNullOrEmpty(group.Key))
    //         {
    //             statistics.DepartmentStatistics.Add(new DepartmentStatistics
    //             {
    //                 DepartmentName = group.Key,
    //                 EmployeeCount = group.Count(),
    //                 TotalSalary = group.Sum(e => e.BasicSalary)
    //             });
    //         }
    //     }
    //
    //     return statistics;
    // }
}
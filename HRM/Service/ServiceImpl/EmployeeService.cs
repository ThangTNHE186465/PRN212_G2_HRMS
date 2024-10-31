using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using HRM.Models;
using HRM.Models.Enum;
using HRM.Repositories;
using HRM.Repositories.RepositoryImpl;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;

namespace HRM.Service.ServiceImpl;

public class EmployeeService : IEmployeeService
{
    private readonly EmployeeRepository _employeeRepository;
    private readonly ActivityLogService _activityLogService;
    private readonly ILogger<EmployeeService> _logger;

    public EmployeeService()
    {
        _employeeRepository = new EmployeeRepository(new HrmContext());
        _activityLogService = new ActivityLogService();
        _logger = new Logger<EmployeeService>(new LoggerFactory());
    }

    public async Task<Employee?> GetByIdAsync(int id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);
        if (employee == null)
        {
            throw new KeyNotFoundException($"Employee with ID {id} not found");
        }
        return employee;
    }

    public async Task<Employee?> GetByUserId(int userId)
    {
        return await _employeeRepository.GetQueryable()
            .FirstOrDefaultAsync(e => e.UserId == userId);
    }

    public Task<Employee?> GetByEmployeeCodeAsync(string code)
    {
        return _employeeRepository.GetQueryable()
            .FirstOrDefaultAsync(e => ("EMP" + e.Id.ToString().PadLeft(5, '0')) == code);
    }

    public async Task<IEnumerable<Employee?>> GetAllEmployees()
    {
        return await _employeeRepository.GetAllAsync();
    }

    public async Task<Employee?> CreateEmployeeAsync(Employee? employee)
    {
        try
        {
            // Validate required fields
            ValidateEmployee(employee);

            // Set default values for new employee
            employee.Status = EmployeeStatus.Active;
            employee.HireDate = employee.HireDate == default ? DateOnly.FromDateTime(DateTime.Now) : employee.HireDate;

            // Save employee
            var createdEmployee = await _employeeRepository.AddAsync(employee);

            // Log activity
            // await _activityLogService.LogActivityAsync(
            //     "Employee Created",
            //     $"Created employee: {employee.FirstName + " " + employee.LastName} ({employee.Id})"
            // );

            return createdEmployee;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating employee: {EmployeeCode}", employee.EmployeeCode);
            throw;
        }
    }

    public async Task<Employee?> UpdateEmployeeAsync(int id, Employee? updatedEmployee)
    {
        var existingEmployee = await GetByIdAsync(id);

        try
        {
            ValidateEmployee(updatedEmployee);

            updatedEmployee.Id = existingEmployee.Id;
            updatedEmployee.ModifiedDate = DateTime.Now;
            updatedEmployee.PhotoPath = existingEmployee.PhotoPath;

            await _employeeRepository.UpdateAsync(updatedEmployee);

            // await _activityLogService.LogActivityAsync(
            //     "Employee Updated",
            //     $"Updated employee: {updatedEmployee.FullName} ({updatedEmployee.EmployeeCode})"
            // );

            return updatedEmployee;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating employee: {Id}", id);
            throw;
        }
    }

    public async Task DeleteEmployeeAsync(int id)
    {
        await _employeeRepository.DeleteAsync(id);
    }

    public Task<IEnumerable<Employee>> SearchEmployeesAsync(string? searchTerm,
        int? departmentId,
        DateTime? startDate,
        DateTime? endDate)
    {
        return _employeeRepository.SearchEmployeesAsync(searchTerm, departmentId, startDate, endDate);
    }

    public async Task<bool> UpdateEmployeeStatusAsync(int id, EmployeeStatus status)
    {

        Employee? emp = await _employeeRepository.GetByIdAsync(id);
        if (emp == null)
        {
            throw new KeyNotFoundException($"Employee with ID {id} not found");
        }
        emp.Status = status;
        await _employeeRepository.UpdateAsync(emp);
        return true;
    }

    public async Task<IEnumerable<Employee?>> FilterEmployeesAsync(string? selectedGender, string selectedSalaryRange, DateTime? startDate, DateTime? endDate)
    {
        var query = _employeeRepository.GetQueryable();

        if (!string.IsNullOrEmpty(selectedGender) && Enum.TryParse<Gender>(selectedGender, out var gender))
        {
            query = query.Where(e => e.Gender == gender);
        }

        if (!string.IsNullOrEmpty(selectedSalaryRange))
        {
            var salaryRange = selectedSalaryRange.Split('-').Select(decimal.Parse).ToArray();
            query = query.Where(e => e.BasicSalary >= salaryRange[0] && e.BasicSalary <= salaryRange[1]);
        }

        if (startDate.HasValue)
        {
            query = query.Where(e => e.HireDate >= DateOnly.FromDateTime(startDate.Value));
        }

        if (endDate.HasValue)
        {
            query = query.Where(e => e.HireDate <= DateOnly.FromDateTime(endDate.Value));
        }

        return await query.ToListAsync();
    }

    public async Task<decimal> CalculateTotalSalaryAsync(int employeeId, int month, int year)
    {
        // var employee = await _employeeRepository.GetByIdAsync(employeeId);
        // if (employee == null)
        // {
        //     throw new KeyNotFoundException($"Employee with ID {employeeId} not found");
        // }
        //
        // // Assuming you have a method to calculate the salary based on the month and year
        // var totalSalary = await _salaryCalculationService.CalculateTotalSalaryAsync(employee, new DateTime(year, month, 1));
        // return totalSalary;
        return 0;
    }


    public async Task<bool> UploadAvatarAsync(int id, string avatar)
    {
        var employee = await GetByIdAsync(id);

        try
        {
            // Update employee record
            employee.PhotoPath = avatar;
            await _employeeRepository.UpdateAsync(employee);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading avatar for employee: {Id}", id);
            throw;
        }
    }

    public async Task<byte[]> ExportToExcelAsync(ObservableCollection<Employee?> employees)
    {
        try
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Employees");

            // Add headers
            var headers = new string[]
            {
                "Employee Code",
                "Full Name",
                "Department",
                "Phone",
                "Email",
                "Basic Salary",
                "Date of Birth",
                "Hired Date",
                "Status"
            };

            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cells[1, i + 1].Value = headers[i];
                worksheet.Cells[1, i + 1].Style.Font.Bold = true;
            }

            // Add data
            int row = 2;
            foreach (var employee in employees)
            {
                worksheet.Cells[row, 1].Value = employee.EmployeeCode;
                worksheet.Cells[row, 2].Value = employee.FullName;
                worksheet.Cells[row, 3].Value = employee.Department?.Name;
                worksheet.Cells[row, 4].Value = employee.Phone;
                worksheet.Cells[row, 5].Value = employee.Email;
                worksheet.Cells[row, 6].Value = employee.BasicSalary;
                worksheet.Cells[row, 7].Value = employee.DateOfBirth;
                worksheet.Cells[row, 8].Value = employee.HireDate;
                worksheet.Cells[row, 9].Value = employee.Status;
                row++;
            }

            worksheet.Cells.AutoFitColumns();

            return await package.GetAsByteArrayAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting employees to Excel");
            throw;
        }
    }

    private void ValidateEmployee(Employee? employee)
    {
        var validationErrors = new List<string>();

        if (string.IsNullOrEmpty(employee.FirstName))
            validationErrors.Add("First name is required");

        if (string.IsNullOrEmpty(employee.LastName))
            validationErrors.Add("Last name is required");

        if (employee.DateOfBirth == default)
            validationErrors.Add("Date of birth is required");

        if (employee.DateOfBirth > DateOnly.FromDateTime(DateTime.Now.AddYears(-18)))
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

    public async Task<EmployeeStatistics> GetEmployeeStatisticsAsync()
    {
        var statistics = new EmployeeStatistics
        {
            TotalEmployees = _employeeRepository.GetAllAsync().Result.Count(),
            ActiveEmployees = (await _employeeRepository.GetActiveEmployeesAsync()).Count(),
            DepartmentStatistics = new List<DepartmentStatistics>()
        };

        var employees = await _employeeRepository.GetActiveEmployeesAsync();
        var departmentGroups = employees.GroupBy(e => e.Department?.Name);

        foreach (var group in departmentGroups)
        {
            if (!string.IsNullOrEmpty(group.Key))
            {
                statistics.DepartmentStatistics.Add(new DepartmentStatistics
                {
                    DepartmentName = group.Key,
                    EmployeeCount = group.Count(),
                    TotalSalary = group.Sum(e => e.BasicSalary)
                });
            }
        }

        return statistics;
    }
}
using HRM.Models;
using HRM.Models.Enum;
using Microsoft.EntityFrameworkCore;

namespace HRM.Repositories.RepositoryImpl;

public class EmployeeRepository(HrmContext context) : BaseRepository<Employee>(context), IEmployeeRepository
{
    public override IQueryable<Employee> AddIncludes(IQueryable<Employee> query)
    {
        return query.Include(e => e.Department)
            .Include(e => e.User)
            .Include(e => e.Attendances)
            .Include(e => e.Salaries)
            .Include(e => e.LeafEmployees)
            .Include(e => e.LeafApprovedByNavigations);
    }

    public async Task<IEnumerable<Employee>> GetEmployeesByDepartmentAsync(int departmentId)
    {
        return await _dbSet
            .Where(e => e.DepartmentId == departmentId)
            .Include(e => e.Department)
            .ToListAsync();
    }

    public async Task<IEnumerable<Employee>> SearchEmployeesAsync(
        string? searchTerm,
        int? departmentId,
        DateTime? startDate,
        DateTime? endDate)
    {
        var query = _dbSet.AsQueryable();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(e =>
                e.FullName.Contains(searchTerm) ||
                e.EmployeeCode.Contains(searchTerm) ||
                e.Phone!.Contains(searchTerm) ||
                e.Address!.Contains(searchTerm) ||
                e.Gender.ToString().Contains(searchTerm) ||
                e.Email.Contains(searchTerm));
        }

        if (departmentId.HasValue && departmentId != 0)
        {
            query = query.Where(e => e.DepartmentId == departmentId);
        }

        if (startDate.HasValue)
        {
            query = query.Where(e => e.HireDate >= DateOnly.FromDateTime(startDate.Value));
        }

        if (endDate.HasValue)
        {
            query = query.Where(e => e.HireDate <= DateOnly.FromDateTime(endDate.Value));
        }

        return await query
            .Include(e => e.Department)
            .OrderBy(e => e.LastName)
            .ToListAsync();
    }

    public async Task<bool> IsExist(string employeeEmployeeCode)
    {
        return await _dbSet.AnyAsync(e => e.EmployeeCode == employeeEmployeeCode);
    }

    public async Task<IEnumerable<Employee>> GetActiveEmployeesAsync()
    {
        return await _dbSet
            .Where(e => e.Status == EmployeeStatus.Active)
            .ToListAsync();
    }
}
using HRM.Models;
using Microsoft.EntityFrameworkCore;

namespace HRM.Repositories.RepositoryImpl;

public class EmployeeRepository(HrmContext context) : BaseRepository<Employee>(context), IEmployeeRepository
{
	public async Task<IEnumerable<Employee>> GetEmployeesByDepartmentAsync(int departmentId)
	{
		return await _dbSet
			.Where(e => e.DepartmentId == departmentId)
			.Include(e => e.Department)
			.ToListAsync();
	}

	public async Task<IEnumerable<Employee>> SearchEmployeesAsync(
		string searchTerm,
		int? departmentId,
		DateTime? startDate,
		DateTime? endDate)
	{
		var query = _dbSet.AsQueryable();

		if (!string.IsNullOrEmpty(searchTerm))
		{
			query = query.Where(e =>
				e.FirstName.Contains(searchTerm) ||
				e.LastName.Contains(searchTerm) ||
				e.Email.Contains(searchTerm));
		}

		if (departmentId.HasValue)
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
}
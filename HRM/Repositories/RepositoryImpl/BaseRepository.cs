using HRM.Models;
using Microsoft.EntityFrameworkCore;

namespace HRM.Repositories.RepositoryImpl;

public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
{
    private readonly HrmContext _context;
    protected readonly DbSet<TEntity?> _dbSet;

    public BaseRepository(HrmContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    public virtual async Task<TEntity?> GetByIdAsync(int id)
    {
        try
        {
            IQueryable<TEntity?> query = _dbSet;
            query = AddIncludes(query);
            return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public virtual async Task<IEnumerable<TEntity?>> GetAllAsync()
    {
        try
        {
            IQueryable<TEntity?> query = _dbSet;
            query = AddIncludes(query);

            return await query.ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public virtual async Task<TEntity?> AddAsync(TEntity? entity)
    {
        try
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public virtual async Task UpdateAsync(TEntity? entity)
    {
        try
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public virtual async Task DeleteAsync(int id)
    {
        try
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public virtual IQueryable<TEntity?> GetQueryable()
    {
        try
        {
            return _dbSet.AsQueryable();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
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
    public virtual IQueryable<TEntity?> AddIncludes(IQueryable<TEntity?> query)
    {
        return query;
    }
}
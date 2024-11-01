using HRM.Models;

namespace HRM.Repositories.RepositoryImpl;

public class DepartmentRepository(HrmContext context) : BaseRepository<Department>(context), IDepartmentRepository
{
    
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;

namespace Repositories
{
    public interface IDepartmentRepository
    {
        List<Department> GetDepartments();
        void AddDepartment(Department department);
        List<Department>? SearchDepartment(string Name);
        void RemoveDepartment(Department department);
        void UpdateDepartment(Department department);

    }
}

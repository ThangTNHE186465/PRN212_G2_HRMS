using BusinessObjects;
using DataAccessObjects;
using System;
using System.Collections.Generic;

namespace Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {

        public void AddDepartment(Department department)
            => DepartmentDAO.AddDepartment(department);

        public List<Department>? SearchDepartment(string name)
            => DepartmentDAO.SearchDepartment(name);

        public void RemoveDepartment(Department department)
            => DepartmentDAO.RemoveDepartment(department);

        public void UpdateDepartment(Department department)
            => DepartmentDAO.UpdateDepartment(department);

        public List<Department> GetDepartments()
            => DepartmentDAO.GetDepartments();

        public int GetEmployeeCountByDepartment(int departmentId)
            => DepartmentDAO.GetEmployeeCountByDepartment(departmentId);

    }
}

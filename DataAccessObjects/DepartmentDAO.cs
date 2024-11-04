using BusinessObjects;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DataAccessObjects
{
    public static class DepartmentDAO
    {
        public static List<Department> GetDepartments()
        {
            using (var context = new FuhrmContext())
            {
                return context.Departments
                    .Include(d => d.Employees)
                    .ToList();
            }
        }

        public static void AddDepartment(Department department)
        {
            using (var context = new FuhrmContext())
            {
                context.Departments.Add(department);
                context.SaveChanges();
            }
        }

        public static List<Department> SearchDepartment(string name)
        {
            using (var context = new FuhrmContext())
            {
                return context.Departments
                    .Where(d => d.DepartmentName.Contains(name))
                    .ToList();
            }
        }

        public static void RemoveDepartment(Department department)
        {
            using (var context = new FuhrmContext())
            {
                context.Departments.Remove(department);
                context.SaveChanges();
            }
        }

        public static void UpdateDepartment(Department department)
        {
            using (var context = new FuhrmContext())
            {
                context.Departments.Update(department);
                context.SaveChanges();
            }
        }

        public static int GetEmployeeCountByDepartment(int departmentId)
        {
            using (var context = new FuhrmContext())
            {
                return context.Employees.Count(e => e.DepartmentId == departmentId);
            }
        }


    }
}

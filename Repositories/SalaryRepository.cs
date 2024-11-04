using BusinessObjects;
using DataAccessObjects;
using System.Collections.Generic;

namespace Repositories
{
    public class SalaryRepository : ISalaryRepository
    {
        public void AddSalary(Salary salary)
            => SalaryDAO.AddSalary(salary);

        public List<Salary>? SearchSalary(string employeeName)
            => SalaryDAO.SearchSalary(employeeName);

        public void RemoveSalary(Salary salary)
            => SalaryDAO.RemoveSalary(salary);

        public void UpdateSalary(Salary salary)
            => SalaryDAO.UpdateSalary(salary);

        public List<Salary> GetSalaries()
            => SalaryDAO.GetSalaries();
    }
}

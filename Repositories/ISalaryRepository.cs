using BusinessObjects;

namespace Repositories
{
    public interface ISalaryRepository
    {
        List<Salary> GetSalaries();
        void AddSalary(Salary salary);
        List<Salary>? SearchSalary(string employeeName);
        void RemoveSalary(Salary salary);
        void UpdateSalary(Salary salary);
    }
}

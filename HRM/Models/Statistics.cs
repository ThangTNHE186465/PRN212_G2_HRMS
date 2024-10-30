namespace HRM.Models;

public partial class EmployeeStatistics
{
    public int TotalEmployees { get; set; }
    public int ActiveEmployees { get; set; }
    public List<DepartmentStatistics> DepartmentStatistics { get; set; }
}

public partial class DepartmentStatistics
{
    public string DepartmentName { get; set; }
    public int EmployeeCount { get; set; }
    public decimal TotalSalary { get; set; }
}
namespace HRM.Models;

public class EmployeeStatistics
{
    public int TotalEmployees { get; set; }
    public int ActiveEmployees { get; set; }
    public List<DepartmentStatistics> DepartmentStatistics { get; set; }
}
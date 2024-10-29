using System;
using System.Collections.Generic;
using HRM.Models.Enum;

namespace HRM.Models;

public partial class Employee
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public DateOnly DateOfBirth { get; set; }

    public DateOnly HireDate { get; set; }

    public string? Address { get; set; }

    public string? PhotoPath { get; set; }

    public int? DepartmentId { get; set; }

    public decimal BasicSalary { get; set; }

    public EmployeeStatus Status { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    public virtual Department? Department { get; set; }

    public virtual ICollection<Leave> LeafApprovedByNavigations { get; set; } = new List<Leave>();

    public virtual ICollection<Leave> LeafEmployees { get; set; } = new List<Leave>();

    public virtual ICollection<Salary> Salaries { get; set; } = new List<Salary>();

    public virtual User User { get; set; } = null!;

    public string FullName => FirstName + " " + LastName;
    public string EmployeeCode => "EMP" + Id.ToString().PadLeft(5, '0');
}
using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class Department
{
    public int DepartmentId { get; set; }

    public string DepartmentName { get; set; } = null!;

    public DateTime? CreateDate { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    // New property for binding purposes
    public int EmployeeCount => Employees.Count;
}

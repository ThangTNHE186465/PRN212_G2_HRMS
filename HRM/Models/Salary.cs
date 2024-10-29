using System;
using System.Collections.Generic;

namespace HRM.Models;

public partial class Salary
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public decimal BaseSalary { get; set; }

    public decimal? Allowance { get; set; }

    public decimal? Bonus { get; set; }

    public decimal? Deduction { get; set; }

    public int Month { get; set; }

    public int Year { get; set; }

    public DateTime? PaymentDate { get; set; }

    public string? Status { get; set; }

    public virtual Employee Employee { get; set; } = null!;
}

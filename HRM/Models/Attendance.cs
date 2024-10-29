using System;
using System.Collections.Generic;

namespace HRM.Models;

public partial class Attendance
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public DateTime CheckInTime { get; set; }

    public DateTime? CheckOutTime { get; set; }

    public decimal? WorkingHours { get; set; }

    public string? Status { get; set; }

    public virtual Employee Employee { get; set; } = null!;
}

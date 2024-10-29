using System;
using System.Collections.Generic;

namespace HRM.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Role { get; set; } = null!;

    public DateTime? CreatedDate { get; set; }

    public DateTime? LastLoginDate { get; set; }

    public virtual ICollection<ActivityLog> ActivityLogs { get; set; } = new List<ActivityLog>();

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ICollection<Notification> NotificationReceiverUsers { get; set; } = new List<Notification>();

    public virtual ICollection<Notification> NotificationSenderUsers { get; set; } = new List<Notification>();
}

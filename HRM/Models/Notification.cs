using System;
using System.Collections.Generic;

namespace HRM.Models;

public partial class Notification
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Message { get; set; } = null!;

    public int SenderUserId { get; set; }

    public int? ReceiverUserId { get; set; }

    public int? DepartmentId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public bool? IsRead { get; set; }

    public virtual Department? Department { get; set; }

    public virtual User? ReceiverUser { get; set; }

    public virtual User SenderUser { get; set; } = null!;
}

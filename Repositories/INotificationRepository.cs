using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;

namespace Repositories
{
    public interface INotificationRepository
    {
        List<Notification> GetNotifications();
        void AddNotification(Notification notification);
        List<Notification>? SearchNotification(string title);
        void RemoveNotification(Notification notification);
        void UpdateNotification(Notification notification);
        List<Notification> GetNotificationsByDepartmentId(int departmentId); // New method
    }
}


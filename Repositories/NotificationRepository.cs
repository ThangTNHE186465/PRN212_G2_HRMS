using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using DataAccessObjects;

namespace Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        public List<Notification> GetNotifications()
            => NotificationDAO.GetNotifications();
        public void AddNotification(Notification notification)
            => NotificationDAO.AddNotification(notification);
        public List<Notification>? SearchNotification(string title)
            => NotificationDAO.SearchNotification(title);
        public void RemoveNotification(Notification notification)
            => NotificationDAO.RemoveNotification(notification);
        public void UpdateNotification(Notification notification)
            => NotificationDAO.UpdateNotification(notification);

        public List<Notification> GetNotificationsByDepartmentId(int departmentId) // Implementing the missing method
            => NotificationDAO.GetNotificationsByDepartmentId(departmentId);
    }
}

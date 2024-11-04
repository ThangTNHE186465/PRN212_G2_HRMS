using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessObjects
{
    public class NotificationDAO
    {
        public static List<Notification> GetNotifications()
        {
            using (var context = new FuhrmContext())
            {
                return context.Notifications
                    .Include(n => n.Department)
                    .ToList();
            }
        }

        public static void AddNotification(Notification notification)
        {
            using (var context = new FuhrmContext())
            {
                context.Notifications.Add(notification);
                context.SaveChanges();
            }
        }

        public static List<Notification>? SearchNotification(string title)
        {
            using (var context = new FuhrmContext())
            {
                return context.Notifications
                    .Where(n => n.Title.Contains(title))
                    .ToList();
            }
        }

        public static void RemoveNotification(Notification notification)
        {
            using (var context = new FuhrmContext())
            {
                context.Notifications.Remove(notification);
                context.SaveChanges();
            }
        }

        public static void UpdateNotification(Notification notification)
        {
            using (var context = new FuhrmContext())
            {
                context.Notifications.Update(notification);
                context.SaveChanges();
            }
        }

        public static List<Notification> GetNotificationsByDepartmentId(int departmentId) // New method
        {
            using (var context = new FuhrmContext())
            {
                return context.Notifications
                    .Where(n => n.DepartmentId == departmentId)
                    .ToList();
            }
        }
    }
}

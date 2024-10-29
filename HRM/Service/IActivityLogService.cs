namespace HRM.Service;

public interface IActivityLogService
{
    Task LogActivityAsync(string employeeCreated, string s);
}
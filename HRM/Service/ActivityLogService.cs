using HRM.Models;
using HRM.Repositories;
using HRM.Repositories.RepositoryImpl;

namespace HRM.Service.ServiceImpl;

public class ActivityLogService() : BaseRepository<ActivityLog>(new HrmContext()), IActivityLogService
{
    private readonly IActivityLogRepository _activityLogRepository = new ActivityLogRepository(new HrmContext());
}
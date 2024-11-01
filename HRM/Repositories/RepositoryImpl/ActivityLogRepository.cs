using HRM.Models;

namespace HRM.Repositories.RepositoryImpl;

public class ActivityLogRepository(HrmContext context) : BaseRepository<ActivityLog>(context), IActivityLogRepository
{

}
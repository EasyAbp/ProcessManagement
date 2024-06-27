using System.Linq;
using EasyAbp.ProcessManagement.Notifications;

namespace EasyAbp.ProcessManagement.EntityFrameworkCore.Notifications;

public static class NotificationEfCoreQueryableExtensions
{
    public static IQueryable<Notification> IncludeDetails(this IQueryable<Notification> queryable, bool include = true)
    {
        if (!include)
        {
            return queryable;
        }

        return queryable;
    }
}

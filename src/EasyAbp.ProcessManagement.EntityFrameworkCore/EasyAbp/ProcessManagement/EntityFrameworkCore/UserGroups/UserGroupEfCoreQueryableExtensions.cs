using System.Linq;
using EasyAbp.ProcessManagement.UserGroups;

namespace EasyAbp.ProcessManagement.EntityFrameworkCore.UserGroups;

public static class UserGroupEfCoreQueryableExtensions
{
    public static IQueryable<UserGroup> IncludeDetails(this IQueryable<UserGroup> queryable, bool include = true)
    {
        if (!include)
        {
            return queryable;
        }

        return queryable;
    }
}

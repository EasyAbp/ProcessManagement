using System.Linq;
using EasyAbp.ProcessManagement.ProcessStateHistories;

namespace EasyAbp.ProcessManagement.EntityFrameworkCore.ProcessStateHistories;

public static class ProcessStateHistoryEfCoreQueryableExtensions
{
    public static IQueryable<ProcessStateHistory> IncludeDetails(this IQueryable<ProcessStateHistory> queryable,
        bool include = true)
    {
        if (!include)
        {
            return queryable;
        }

        return queryable;
    }
}
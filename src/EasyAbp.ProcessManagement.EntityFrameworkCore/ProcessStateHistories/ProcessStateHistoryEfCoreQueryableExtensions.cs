using System.Linq;

namespace EasyAbp.ProcessManagement.ProcessStateHistories;

public static class ProcessStateHistoryEfCoreQueryableExtensions
{
    public static IQueryable<ProcessStateHistory> IncludeDetails(this IQueryable<ProcessStateHistory> queryable, bool include = true)
    {
        if (!include)
        {
            return queryable;
        }

        return queryable
            // .Include(x => x.xxx) // TODO: AbpHelper generated
            ;
    }
}

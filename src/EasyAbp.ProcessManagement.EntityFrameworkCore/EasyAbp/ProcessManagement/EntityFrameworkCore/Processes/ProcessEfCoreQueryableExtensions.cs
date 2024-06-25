using System.Linq;

namespace EasyAbp.ProcessManagement.Processes;

public static class ProcessEfCoreQueryableExtensions
{
    public static IQueryable<Process> IncludeDetails(this IQueryable<Process> queryable, bool include = true)
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

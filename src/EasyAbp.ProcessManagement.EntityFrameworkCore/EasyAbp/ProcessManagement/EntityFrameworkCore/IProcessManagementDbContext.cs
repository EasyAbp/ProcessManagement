using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace EasyAbp.ProcessManagement.EntityFrameworkCore;

[ConnectionStringName(ProcessManagementDbProperties.ConnectionStringName)]
public interface IProcessManagementDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
}
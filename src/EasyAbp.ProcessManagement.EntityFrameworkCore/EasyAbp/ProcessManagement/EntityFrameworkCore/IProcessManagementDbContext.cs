using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using EasyAbp.ProcessManagement.Processes;
using EasyAbp.ProcessManagement.ProcessStateHistories;

namespace EasyAbp.ProcessManagement.EntityFrameworkCore;

[ConnectionStringName(ProcessManagementDbProperties.ConnectionStringName)]
public interface IProcessManagementDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
    DbSet<Process> Processes { get; set; }
    DbSet<ProcessStateHistory> ProcessStateHistories { get; set; }
}

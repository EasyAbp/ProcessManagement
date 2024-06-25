using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using EasyAbp.ProcessManagement.Processes;
using EasyAbp.ProcessManagement.ProcessStateHistories;

namespace EasyAbp.ProcessManagement.EntityFrameworkCore;

[ConnectionStringName(ProcessManagementDbProperties.ConnectionStringName)]
public class ProcessManagementDbContext : AbpDbContext<ProcessManagementDbContext>, IProcessManagementDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */
    public DbSet<Process> Processes { get; set; }
    public DbSet<ProcessStateHistory> ProcessStateHistories { get; set; }

    public ProcessManagementDbContext(DbContextOptions<ProcessManagementDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureProcessManagement();
    }
}

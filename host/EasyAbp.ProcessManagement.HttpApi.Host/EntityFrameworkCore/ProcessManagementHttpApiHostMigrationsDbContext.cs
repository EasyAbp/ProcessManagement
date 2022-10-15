using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace EasyAbp.ProcessManagement.EntityFrameworkCore;

public class ProcessManagementHttpApiHostMigrationsDbContext : AbpDbContext<ProcessManagementHttpApiHostMigrationsDbContext>
{
    public ProcessManagementHttpApiHostMigrationsDbContext(DbContextOptions<ProcessManagementHttpApiHostMigrationsDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureProcessManagement();
    }
}

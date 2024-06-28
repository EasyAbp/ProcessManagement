using EasyAbp.ProcessManagement.Notifications;
using EasyAbp.ProcessManagement.UserGroups;
using EasyAbp.ProcessManagement.ProcessStateHistories;
using EasyAbp.ProcessManagement.Processes;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace EasyAbp.ProcessManagement.EntityFrameworkCore;

public static class ProcessManagementDbContextModelCreatingExtensions
{
    public static void ConfigureProcessManagement(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        /* Configure all entities here. Example:

        builder.Entity<Question>(b =>
        {
            //Configure table & schema name
            b.ToTable(ProcessManagementDbProperties.DbTablePrefix + "Questions", ProcessManagementDbProperties.DbSchema);

            b.ConfigureByConvention();

            //Properties
            b.Property(q => q.Title).IsRequired().HasMaxLength(QuestionConsts.MaxTitleLength);

            //Relations
            b.HasMany(question => question.Tags).WithOne().HasForeignKey(qt => qt.QuestionId);

            //Indexes
            b.HasIndex(q => q.CreationTime);
        });
        */

        builder.Entity<Process>(b =>
        {
            b.ToTable(ProcessManagementDbProperties.DbTablePrefix + "Processes",
                ProcessManagementDbProperties.DbSchema);
            b.ConfigureByConvention();

            /* Configure more properties here */
            b.HasIndex(x => new { x.GroupKey, x.ProcessName });
            b.HasIndex(x => x.CorrelationId);
        });

        builder.Entity<ProcessStateHistory>(b =>
        {
            b.ToTable(ProcessManagementDbProperties.DbTablePrefix + "ProcessStateHistories",
                ProcessManagementDbProperties.DbSchema);
            b.ConfigureByConvention();

            /* Configure more properties here */
            b.HasIndex(x => new { x.ProcessId, x.StateUpdateTime });
        });

        builder.Entity<UserGroup>(b =>
        {
            b.ToTable(ProcessManagementDbProperties.DbTablePrefix + "UserGroups",
                ProcessManagementDbProperties.DbSchema);
            b.ConfigureByConvention();

            /* Configure more properties here */
            b.HasIndex(x => x.UserId);
            b.HasIndex(x => x.GroupKey);
        });

        builder.Entity<Notification>(b =>
        {
            b.ToTable(ProcessManagementDbProperties.DbTablePrefix + "Notifications",
                ProcessManagementDbProperties.DbSchema);
            b.ConfigureByConvention();

            /* Configure more properties here */
            b.HasIndex(x => x.ProcessId);
            b.HasIndex(x => new { x.CreationTime, x.UserId });
        });
    }
}
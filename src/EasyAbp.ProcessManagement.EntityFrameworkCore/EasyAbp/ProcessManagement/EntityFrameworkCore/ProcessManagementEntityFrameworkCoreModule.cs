using EasyAbp.ProcessManagement.EntityFrameworkCore.Processes;
using EasyAbp.ProcessManagement.EntityFrameworkCore.ProcessStateHistories;
using EasyAbp.ProcessManagement.ProcessStateHistories;
using EasyAbp.ProcessManagement.Processes;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace EasyAbp.ProcessManagement.EntityFrameworkCore;

[DependsOn(
    typeof(ProcessManagementDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class ProcessManagementEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<ProcessManagementDbContext>(options =>
        {
            /* Add custom repositories here. Example:
             * options.AddRepository<Question, EfCoreQuestionRepository>();
             */
            options.AddRepository<Process, ProcessRepository>();
            options.AddRepository<ProcessStateHistory, ProcessStateHistoryRepository>();
        });
    }
}

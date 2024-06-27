using EasyAbp.ProcessManagement.Processes;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Domain;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.Modularity;
using Volo.Abp.Users;

namespace EasyAbp.ProcessManagement;

[DependsOn(
    typeof(AbpAutoMapperModule),
    typeof(AbpDddDomainModule),
    typeof(AbpUsersAbstractionModule),
    typeof(ProcessManagementDomainSharedModule)
)]
public class ProcessManagementDomainModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<ProcessManagementDomainModule>();
        Configure<AbpAutoMapperOptions>(options => { options.AddMaps<ProcessManagementDomainModule>(validate: true); });

        Configure<AbpDistributedEntityEventOptions>(options =>
        {
            options.AutoEventSelectors.Add<Process>();
            options.EtoMappings.Add<Process, ProcessEto>();
        });
    }
}
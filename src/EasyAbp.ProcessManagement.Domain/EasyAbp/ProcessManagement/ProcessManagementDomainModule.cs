using EasyAbp.ProcessManagement.Localization;
using EasyAbp.ProcessManagement.Options;
using EasyAbp.ProcessManagement.Processes;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Domain;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.Localization;
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
            options.EtoMappings.Add<Process, ProcessEto>(typeof(ProcessManagementDomainModule));
        });

        Configure<ProcessManagementOptions>(options =>
        {
            var definition = new ProcessDefinition(
                    ProcessManagementConsts.InstantNotificationProcess.ProcessName,
                    ProcessManagementConsts.InstantNotificationProcess.ProcessDisplayName)
                .AddState(new ProcessStateDefinition(
                    ProcessManagementConsts.InstantNotificationProcess.TheOnlyStateName,
                    ProcessManagementConsts.InstantNotificationProcess.TheOnlyStateDisplayName,
                    null, ProcessStateFlag.Information));

            options.AddOrUpdateProcessDefinition(definition);
        });
    }
}
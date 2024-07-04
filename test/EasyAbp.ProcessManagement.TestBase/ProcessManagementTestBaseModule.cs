using EasyAbp.ProcessManagement.Options;
using EasyAbp.ProcessManagement.Processes;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Authorization;
using Volo.Abp.Autofac;
using Volo.Abp.Data;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;

namespace EasyAbp.ProcessManagement;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpTestBaseModule),
    typeof(AbpAuthorizationModule),
    typeof(ProcessManagementDomainModule)
)]
public class ProcessManagementTestBaseModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAlwaysAllowAuthorization();

        ConfigureDemoProcessDefinitions(context);
    }

    private void ConfigureDemoProcessDefinitions(ServiceConfigurationContext context)
    {
        /*                           Succeeded
         *                        ↗
         *             Exporting
         *          ↗             ↘
         *   Ready                   ExportFailed
         *          ↘
         *             FailedToStartExporting
         */
        var processDefinition = new ProcessDefinition("FakeExport", new LocalizableString("Process:FakeExport"))
            .AddState(new ProcessStateDefinition(
                "Ready", new LocalizableString("Process:Ready"),
                null, ProcessStateFlag.Information))
            .AddState(new ProcessStateDefinition(
                "FailedToStartExporting", new LocalizableString("Process:FailedToStartExporting"),
                "Ready", ProcessStateFlag.Failure))
            .AddState(new ProcessStateDefinition(
                "Exporting", new LocalizableString("Process:Exporting"),
                "Ready", ProcessStateFlag.Running))
            .AddState(new ProcessStateDefinition(
                "ExportFailed", new LocalizableString("Process:ExportFailed"),
                "Exporting", ProcessStateFlag.Failure))
            .AddState(new ProcessStateDefinition(
                "Succeeded", new LocalizableString("Process:Succeeded"),
                "Exporting", ProcessStateFlag.Success));

        context.Services.Configure<ProcessManagementOptions>(options =>
        {
            options.AddOrUpdateProcessDefinition(processDefinition);
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        SeedTestData(context);
    }

    private static void SeedTestData(ApplicationInitializationContext context)
    {
        AsyncHelper.RunSync(async () =>
        {
            using (var scope = context.ServiceProvider.CreateScope())
            {
                await scope.ServiceProvider
                    .GetRequiredService<IDataSeeder>()
                    .SeedAsync();
            }
        });
    }
}
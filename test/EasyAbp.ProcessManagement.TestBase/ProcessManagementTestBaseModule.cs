using EasyAbp.ProcessManagement.Options;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Authorization;
using Volo.Abp.Autofac;
using Volo.Abp.Data;
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
        var processDefinition = new ProcessDefinition("FakeExport", "Fake export")
            .AddState(new ProcessStateDefinition("Ready", "Ready", null))
            .AddState(new ProcessStateDefinition("FailedToStartExporting", "Failed", "Ready"))
            .AddState(new ProcessStateDefinition("Exporting", "Exporting", "Ready"))
            .AddState(new ProcessStateDefinition("ExportFailed", "Failed", "Exporting"))
            .AddState(new ProcessStateDefinition("Succeeded", "Succeeded", "Exporting"));

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
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
        var processDefinition = new ProcessDefinition("MyDemoProcess", "My Demo Process")
            .AddState(new ProcessStateDefinition("Startup", "Startup"))
            .AddState(new ProcessStateDefinition("Step1", "Step1"), ["Startup"])
            .AddState(new ProcessStateDefinition("Step2", "Step2"), ["Startup"])
            .AddState(new ProcessStateDefinition("Step3", "Step3"), ["Step1", "Step2"])
            .AddState(new ProcessStateDefinition("Step4", "Step4"), ["Step3", "Step6"])
            .AddState(new ProcessStateDefinition("Step5", "Step5"), ["Step3", "Step8"])
            .AddState(new ProcessStateDefinition("Step6", "Step6"), ["Step4"])
            .AddState(new ProcessStateDefinition("Step7", "Step7"), ["Step5"])
            .AddState(new ProcessStateDefinition("Step8", "Step8"), ["Step7"]);

        //            Step1            Step4 ⇌ Step6
        //         ↗       ↘      ↗
        // Startup             Step3
        //         ↘       ↗      ↘
        //            Step2            Step5 ➔ Step7
        //                                 ↖  ↙
        //                                  Step8

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
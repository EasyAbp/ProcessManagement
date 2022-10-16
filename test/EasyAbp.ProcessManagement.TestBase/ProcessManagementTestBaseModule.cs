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
            .AddState(new ProcessStateDefinition("Startup"))
            .AddState(new ProcessStateDefinition("Step1"))
            .AddState(new ProcessStateDefinition("Step2"))
            .AddState(new ProcessStateDefinition("Step3"))
            .AddState(new ProcessStateDefinition("Step4"))
            .AddState(new ProcessStateDefinition("Step5"))
            .AddState(new ProcessStateDefinition("Step6"))
            .AddState(new ProcessStateDefinition("Step7"))
            .AddState(new ProcessStateDefinition("Step8"));

        processDefinition.SetInitialState("Startup");
        processDefinition.SetNextStates("Startup", new[] { "Step1", "Step2" });
        processDefinition.SetNextStates("Step1", new[] { "Step3" });
        processDefinition.SetNextStates("Step2", new[] { "Step3" });
        processDefinition.SetNextStates("Step3", new[] { "Step4", "Step5" });
        processDefinition.SetNextStates("Step4", new[] { "Step6" });
        processDefinition.SetNextStates("Step5", new[] { "Step7" });
        processDefinition.SetNextStates("Step6", new[] { "Step4" });
        processDefinition.SetNextStates("Step7", new[] { "Step8" });
        processDefinition.SetNextStates("Step8", new[] { "Step5" });

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
using Localization.Resources.AbpUi;
using EasyAbp.ProcessManagement.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace EasyAbp.ProcessManagement;

[DependsOn(
    typeof(ProcessManagementApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class ProcessManagementHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(ProcessManagementHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<ProcessManagementResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}

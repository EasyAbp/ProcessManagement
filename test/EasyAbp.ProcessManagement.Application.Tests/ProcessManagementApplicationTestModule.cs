using EasyAbp.ProcessManagement.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace EasyAbp.ProcessManagement;

[DependsOn(
    typeof(ProcessManagementApplicationModule),
    typeof(ProcessManagementDomainTestModule)
    )]
public class ProcessManagementApplicationTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // Still grants everything by default, but lets a test deny single policies.
        context.Services.AddSingleton<FakeAuthorizationService>();
        context.Services.Replace(ServiceDescriptor.Singleton<IAuthorizationService>(
            sp => sp.GetRequiredService<FakeAuthorizationService>()));
        context.Services.Replace(ServiceDescriptor.Singleton<IAbpAuthorizationService>(
            sp => sp.GetRequiredService<FakeAuthorizationService>()));
    }
}

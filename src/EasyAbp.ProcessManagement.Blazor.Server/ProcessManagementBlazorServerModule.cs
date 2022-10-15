using Volo.Abp.AspNetCore.Components.Server.Theming;
using Volo.Abp.Modularity;

namespace EasyAbp.ProcessManagement.Blazor.Server;

[DependsOn(
    typeof(AbpAspNetCoreComponentsServerThemingModule),
    typeof(ProcessManagementBlazorModule)
    )]
public class ProcessManagementBlazorServerModule : AbpModule
{

}

using Volo.Abp.AspNetCore.Components.WebAssembly.Theming;
using Volo.Abp.Modularity;

namespace EasyAbp.ProcessManagement.Blazor.WebAssembly;

[DependsOn(
    typeof(ProcessManagementBlazorModule),
    typeof(ProcessManagementHttpApiClientModule),
    typeof(AbpAspNetCoreComponentsWebAssemblyThemingModule)
    )]
public class ProcessManagementBlazorWebAssemblyModule : AbpModule
{

}

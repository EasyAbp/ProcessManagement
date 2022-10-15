using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace EasyAbp.ProcessManagement;

[DependsOn(
    typeof(ProcessManagementApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class ProcessManagementHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(ProcessManagementApplicationContractsModule).Assembly,
            ProcessManagementRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<ProcessManagementHttpApiClientModule>();
        });

    }
}

using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace EasyAbp.ProcessManagement;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class ProcessManagementInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<ProcessManagementInstallerModule>();
        });
    }
}

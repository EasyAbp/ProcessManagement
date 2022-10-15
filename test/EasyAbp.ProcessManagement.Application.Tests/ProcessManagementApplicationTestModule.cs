using Volo.Abp.Modularity;

namespace EasyAbp.ProcessManagement;

[DependsOn(
    typeof(ProcessManagementApplicationModule),
    typeof(ProcessManagementDomainTestModule)
    )]
public class ProcessManagementApplicationTestModule : AbpModule
{

}

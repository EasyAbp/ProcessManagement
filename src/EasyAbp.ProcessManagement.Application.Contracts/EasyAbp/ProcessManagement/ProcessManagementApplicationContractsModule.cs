using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace EasyAbp.ProcessManagement;

[DependsOn(
    typeof(ProcessManagementDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class ProcessManagementApplicationContractsModule : AbpModule
{

}

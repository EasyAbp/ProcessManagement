using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace EasyAbp.ProcessManagement;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(ProcessManagementDomainSharedModule)
)]
public class ProcessManagementDomainModule : AbpModule
{

}

using Volo.Abp.Domain;
using Volo.Abp.Modularity;
using Volo.Abp.Users;

namespace EasyAbp.ProcessManagement;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(AbpUsersAbstractionModule),
    typeof(ProcessManagementDomainSharedModule)
)]
public class ProcessManagementDomainModule : AbpModule
{
}
using EasyAbp.ProcessManagement.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace EasyAbp.ProcessManagement;

/* Domain tests are configured to use the EF Core provider.
 * You can switch to MongoDB, however your domain tests should be
 * database independent anyway.
 */
[DependsOn(
    typeof(ProcessManagementEntityFrameworkCoreTestModule)
    )]
public class ProcessManagementDomainTestModule : AbpModule
{

}

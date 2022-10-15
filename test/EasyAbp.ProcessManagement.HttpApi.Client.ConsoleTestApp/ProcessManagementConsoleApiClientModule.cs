using Volo.Abp.Autofac;
using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace EasyAbp.ProcessManagement;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(ProcessManagementHttpApiClientModule),
    typeof(AbpHttpClientIdentityModelModule)
    )]
public class ProcessManagementConsoleApiClientModule : AbpModule
{

}

using Volo.Abp.Data;
using Volo.Abp.Modularity;

namespace EasyAbp.ProcessManagement.MongoDB;

[DependsOn(
    typeof(ProcessManagementTestBaseModule),
    typeof(ProcessManagementMongoDbModule)
    )]
public class ProcessManagementMongoDbTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpDbConnectionOptions>(options =>
        {
            options.ConnectionStrings.Default = MongoDbFixture.GetRandomConnectionString();
        });
    }
}

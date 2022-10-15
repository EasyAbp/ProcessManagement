using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace EasyAbp.ProcessManagement.MongoDB;

[ConnectionStringName(ProcessManagementDbProperties.ConnectionStringName)]
public class ProcessManagementMongoDbContext : AbpMongoDbContext, IProcessManagementMongoDbContext
{
    /* Add mongo collections here. Example:
     * public IMongoCollection<Question> Questions => Collection<Question>();
     */

    protected override void CreateModel(IMongoModelBuilder modelBuilder)
    {
        base.CreateModel(modelBuilder);

        modelBuilder.ConfigureProcessManagement();
    }
}

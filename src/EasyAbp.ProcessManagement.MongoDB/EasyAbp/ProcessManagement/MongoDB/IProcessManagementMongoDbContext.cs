using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace EasyAbp.ProcessManagement.MongoDB;

[ConnectionStringName(ProcessManagementDbProperties.ConnectionStringName)]
public interface IProcessManagementMongoDbContext : IAbpMongoDbContext
{
    /* Define mongo collections here. Example:
     * IMongoCollection<Question> Questions { get; }
     */
}
